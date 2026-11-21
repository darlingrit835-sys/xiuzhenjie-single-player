#!/usr/bin/env python3
import argparse
import base64
import hashlib
import hmac
import json
import os
from pathlib import Path
import re
import shutil
import struct
import subprocess
import tempfile
from urllib.parse import urlsplit
import zipfile


MANIFEST = "manifest.json"
PAYLOAD = "XZJ.HotUpdate.dll.bytes"
SIGNATURE = "manifest.sig"
PASSPHRASE_ENV = "XZJ_HOT_UPDATE_KEY_PASSPHRASE"
MANIFEST_LIMIT = 16 * 1024
PAYLOAD_LIMIT = 8 * 1024 * 1024
HEX_DIGEST = re.compile(r"[0-9a-f]{64}")
TARGET_NAME = re.compile(r"[A-Za-z][A-Za-z0-9]{0,63}")
METADATA_NAME = re.compile(r"[A-Za-z0-9_.-]{1,116}\.dll")
FULL_MANIFEST_LIMIT = 256 * 1024
FULL_PAYLOAD_LIMIT = 1024 * 1024 * 1024
FULL_ARCHIVE = "client-release.zip"
FULL_ASSEMBLIES = {"Assembly-CSharp.dll", "XiuZhenJie.Client.Protocol.dll", "XZJ.HotUpdate.dll"}
FULL_REQUIRED = FULL_ASSEMBLIES | {"xzj-content", "xzj-scenes", "Assetbundles/assetbundleconfig"}
LEGACY_ASSET = re.compile(r"Assetbundles/[A-Za-z0-9_.-]{1,150}")


def read_nonempty(path, limit):
    with path.open("rb") as file:
        data = file.read(limit + 1)
    if not data or len(data) > limit:
        raise ValueError(f"{path.name} 大小超出允许范围。")
    return data


def unique_fields(pairs):
    result = {}
    for key, value in pairs:
        if key in result:
            raise ValueError("清单包含重复字段。")
        result[key] = value
    return result


def read_patch(directory):
    raw = read_nonempty(directory / MANIFEST, MANIFEST_LIMIT)
    manifest = json.loads(raw.decode("utf-8"), object_pairs_hook=unique_fields)
    if not isinstance(manifest, dict) or type(manifest.get("schema")) is not int or manifest["schema"] != 1:
        raise ValueError("热更新清单要求 schema 为 1。")
    target, version = manifest.get("target"), manifest.get("version")
    if not isinstance(target, str) or not TARGET_NAME.fullmatch(target):
        raise ValueError("热更新目标平台字段无效。")
    if type(version) is not int or not 1 <= version <= 2_147_483_647:
        raise ValueError("热更新版本须为正的 32 位整数。")
    for field in ("compatibility", "dllSha256"):
        value = manifest.get(field)
        if not isinstance(value, str) or not HEX_DIGEST.fullmatch(value):
            raise ValueError(f"{field} 须为小写 SHA-256。")
    metadata = manifest.get("metadata")
    if metadata is not None and (
        not isinstance(metadata, list) or len(metadata) > 32
        or any(not isinstance(name, str) or not METADATA_NAME.fullmatch(name) for name in metadata)
        or len(set(metadata)) != len(metadata)
    ):
        raise ValueError("AOT 元数据文件名列表无效。")

    dll = read_nonempty(directory / PAYLOAD, PAYLOAD_LIMIT)
    if not hmac.compare_digest(hashlib.sha256(dll).hexdigest(), manifest["dllSha256"]):
        raise ValueError("DLL 字节的 SHA-256 与清单不符。")
    check_managed_dll(dll)
    return raw


def hash_stream(file):
    digest = hashlib.sha256()
    for chunk in iter(lambda: file.read(1024 * 1024), b""):
        digest.update(chunk)
    return digest.hexdigest()


def is_digest(value):
    return isinstance(value, str) and HEX_DIGEST.fullmatch(value) is not None


def read_full_patch(directory):
    raw = read_nonempty(directory / MANIFEST, FULL_MANIFEST_LIMIT)
    manifest = json.loads(raw.decode("utf-8"), object_pairs_hook=unique_fields)
    if not isinstance(manifest, dict) or type(manifest.get("schema")) is not int or manifest["schema"] != 2:
        raise ValueError("全量发布清单要求 schema 为 2。")
    target, version = manifest.get("target"), manifest.get("version")
    if not isinstance(target, str) or not TARGET_NAME.fullmatch(target):
        raise ValueError("全量发布目标平台无效。")
    if type(version) is not int or not 1 <= version <= 2_147_483_647:
        raise ValueError("全量发布版本须为正的 32 位整数。")
    size = manifest.get("archiveSize")
    if (not is_digest(manifest.get("compatibility")) or not is_digest(manifest.get("archiveSha256"))
        or type(size) is not int or not 1 <= size <= FULL_PAYLOAD_LIMIT):
        raise ValueError("全量发布基线或压缩包信息无效。")
    files = manifest.get("files")
    if not isinstance(files, list) or not 1 <= len(files) <= 1024:
        raise ValueError("全量发布文件清单无效。")

    expected = {}
    folded = set()
    total = 0
    for item in files:
        if not isinstance(item, dict):
            raise ValueError("全量发布文件记录无效。")
        name, length = item.get("name"), item.get("size")
        if (not isinstance(name, str) or (name not in FULL_REQUIRED and (
            not LEGACY_ASSET.fullmatch(name) or name.rsplit("/", 1)[-1] in {".", ".."}))
            or name.lower() in folded or not is_digest(item.get("sha256"))
            or type(length) is not int or not 1 <= length <= FULL_PAYLOAD_LIMIT):
            raise ValueError("全量发布包含无效或重复的文件。")
        folded.add(name.lower())
        expected[name] = item
        total += length
    if total > FULL_PAYLOAD_LIMIT or not FULL_REQUIRED.issubset(expected):
        raise ValueError("全量发布超出大小限制或缺少文件。")

    archive = directory / FULL_ARCHIVE
    with archive.open("rb") as source:
        if os.fstat(source.fileno()).st_size != size or hash_stream(source) != manifest["archiveSha256"]:
            raise ValueError("全量压缩包与签名清单不符。")
        source.seek(0)
        with zipfile.ZipFile(source) as contents:
            members = contents.infolist()
            if len(members) != len(expected) or {member.filename for member in members} != set(expected):
                raise ValueError("全量压缩包包含重复、缺失或额外的文件。")
            for member in members:
                item = expected[member.filename]
                if member.file_size != item["size"]:
                    raise ValueError(f"{member.filename} 大小与清单不符。")
                with contents.open(member) as file:
                    if hash_stream(file) != item["sha256"]:
                        raise ValueError(f"{member.filename} SHA-256 与清单不符。")
                if member.filename in FULL_ASSEMBLIES:
                    check_managed_dll(contents.read(member))
    return raw


def export_files(args):
    manifest = json.loads(read_full_patch(args.patch_dir))
    read_nonempty(args.patch_dir / SIGNATURE, 1024)
    args.objects_dir.mkdir(parents=True, exist_ok=True)
    exported = 0
    with zipfile.ZipFile(args.patch_dir / FULL_ARCHIVE) as release:
        for entry in manifest["files"]:
            target = args.objects_dir / entry["sha256"]
            if target.is_file() and target.stat().st_size == entry["size"]:
                with target.open("rb") as source:
                    if hash_stream(source) == entry["sha256"]:
                        continue
            temporary = None
            try:
                with tempfile.NamedTemporaryFile(dir=args.objects_dir, delete=False) as output:
                    temporary = Path(output.name)
                    with release.open(entry["name"]) as source:
                        shutil.copyfileobj(source, output, 1024 * 1024)
                with temporary.open("rb") as source:
                    if hash_stream(source) != entry["sha256"]:
                        raise ValueError(f"{entry['name']} 已变更，无法导出更新文件。")
                os.replace(temporary, target)
                exported += 1
            finally:
                if temporary is not None:
                    temporary.unlink(missing_ok=True)
    print(f"已导出 {exported} 个缺少的更新文件。")


def check_managed_dll(dll):
    invalid = "补丁须是包含 CLR 头的托管 DLL。"
    if len(dll) < 64 or dll[:2] != b"MZ":
        raise ValueError(invalid)
    pe = struct.unpack_from("<I", dll, 0x3C)[0]
    if pe < 64 or pe + 26 > len(dll) or dll[pe:pe + 4] != b"PE\0\0":
        raise ValueError(invalid)
    optional_size, characteristics = struct.unpack_from("<HH", dll, pe + 20)
    optional = pe + 24
    magic = struct.unpack_from("<H", dll, optional)[0]
    directory_offset = {0x10B: 96, 0x20B: 112}.get(magic)
    if directory_offset is None or not characteristics & 0x2000 or optional + optional_size > len(dll):
        raise ValueError(invalid)
    if optional_size < directory_offset + 15 * 8:
        raise ValueError(invalid)
    count = struct.unpack_from("<I", dll, optional + directory_offset - 4)[0]
    address, size = struct.unpack_from("<II", dll, optional + directory_offset + 14 * 8)
    if count < 15 or address == 0 or size < 72:
        raise ValueError(invalid)


def openssl(arguments, data=None):
    try:
        result = subprocess.run(
            ["openssl", *arguments], input=b"" if data is None else data, capture_output=True, check=False,
        )
    except FileNotFoundError as error:
        raise ValueError("需要在 PATH 中安装 OpenSSL。") from error
    if result.returncode != 0:
        raise ValueError("OpenSSL 处理失败，请检查 RSA 密钥格式和口令。")
    return result.stdout


def der_value(data, position, tag):
    if position + 2 > len(data) or data[position] != tag:
        raise ValueError("无效的 RSA 公钥。")
    length = data[position + 1]
    start = position + 2
    if length & 0x80:
        width = length & 0x7F
        if width == 0 or width > 4 or start + width > len(data):
            raise ValueError("无效的 RSA 公钥长度。")
        length = int.from_bytes(data[start:start + width], "big")
        start += width
    end = start + length
    if length == 0 or end > len(data):
        raise ValueError("无效的 RSA 公钥长度。")
    return data[start:end], end


def rsa_public_parts(encoded):
    sequence, end = der_value(encoded, 0, 0x30)
    modulus, position = der_value(sequence, 0, 0x02)
    exponent, after = der_value(sequence, position, 0x02)
    if end != len(encoded) or after != len(sequence) or modulus[0] & 0x80 or exponent[0] & 0x80:
        raise ValueError("无效的 RSA 公钥参数。")
    modulus, exponent = modulus.lstrip(b"\0"), exponent.lstrip(b"\0")
    bits = int.from_bytes(modulus, "big").bit_length()
    power = int.from_bytes(exponent, "big")
    if not 2048 <= bits <= 8192 or power < 3 or power % 2 == 0:
        raise ValueError("RSA 密钥长度须为 2048 至 8192 位。")
    return modulus, exponent


def atomic_write(path, data, mode=0o644):
    path.parent.mkdir(parents=True, exist_ok=True)
    descriptor, name = tempfile.mkstemp(dir=path.parent, prefix=f".{path.name}.")
    temporary = Path(name)
    try:
        with os.fdopen(descriptor, "wb") as file:
            file.write(data)
            file.flush()
            os.fsync(file.fileno())
        temporary.chmod(mode)
        temporary.replace(path)
    finally:
        temporary.unlink(missing_ok=True)


def sign(args):
    read = read_full_patch if args.command == "sign-full" else read_patch
    manifest = read(args.patch_dir)
    key = str(args.private_key.resolve())
    passin = f"env:{PASSPHRASE_ENV}" if PASSPHRASE_ENV in os.environ else "pass:"
    public_der = openssl(["rsa", "-in", key, "-passin", passin, "-RSAPublicKey_out", "-outform", "DER"])
    modulus, _ = rsa_public_parts(public_der)
    signature = openssl(["dgst", "-sha256", "-sign", key, "-passin", passin, "-sigopt", "rsa_padding_mode:pkcs1"], manifest)
    if len(signature) != len(modulus):
        raise ValueError("签名长度与 RSA 密钥不匹配。")
    atomic_write(args.patch_dir / SIGNATURE, signature)


def configure(args):
    value = args.manifest_url
    url = urlsplit(value)
    if (url.scheme != "https" or not url.hostname or url.username is not None
        or url.port == 0 or any(c in value for c in "?#\\")
        or any(c.isspace() or ord(c) < 32 or ord(c) == 127 for c in value)
        or not url.path.endswith("/manifest.json")):
        raise ValueError("更新地址须为以 /manifest.json 结尾的 HTTPS URL。")
    if args.output.resolve() == args.public_key.resolve():
        raise ValueError("配置路径须与输入公钥路径分开。")
    public_der = openssl(["rsa", "-pubin", "-in", str(args.public_key.resolve()), "-RSAPublicKey_out", "-outform", "DER"])
    modulus, exponent = rsa_public_parts(public_der)
    config = {
        "manifestUrl": value,
        "modulus": base64.b64encode(modulus).decode("ascii"),
        "exponent": base64.b64encode(exponent).decode("ascii"),
    }
    if args.certificate_sha256:
        if not is_digest(args.certificate_sha256):
            raise ValueError("HTTPS 证书指纹须为小写 SHA-256。")
        config["certificateSha256"] = args.certificate_sha256
    if args.gateway_capability_file:
        capability = read_nonempty(args.gateway_capability_file, 258).decode("ascii").rstrip("\r\n")
        if not args.certificate_sha256 or not 32 <= len(capability) <= 256 or any(
            not "!" <= char <= "~" for char in capability
        ):
            raise ValueError("云端入口需要证书指纹和有效的访问凭据。")
        config["gatewayCapability"] = capability
        config["useEmailRegistration"] = args.use_email_registration
    mode = 0o600 if args.gateway_capability_file else 0o644
    atomic_write(args.output, (json.dumps(config, indent=2) + "\n").encode("utf-8"), mode)


def main():
    parser = argparse.ArgumentParser(description="修真界 HybridCLR 补丁签名和公钥配置")
    commands = parser.add_subparsers(dest="command", required=True)
    for command, description in (("sign", "签名单 DLL 补丁"), ("sign-full", "签名全量客户端发布")):
        signer = commands.add_parser(command, help=description)
        signer.add_argument("--patch-dir", type=Path, required=True)
        signer.add_argument("--private-key", type=Path, required=True)
    exporter = commands.add_parser("export-files", help="从已签名的发布包导出按 SHA-256 索引的文件")
    exporter.add_argument("--patch-dir", type=Path, required=True)
    exporter.add_argument("--objects-dir", type=Path, required=True)
    config = commands.add_parser("configure", help="生成客户端 config.json")
    config.add_argument("--public-key", type=Path, required=True)
    config.add_argument("--manifest-url", required=True)
    config.add_argument("--output", type=Path, required=True)
    config.add_argument("--certificate-sha256")
    config.add_argument("--gateway-capability-file", type=Path)
    config.add_argument("--use-email-registration", action=argparse.BooleanOptionalAction, default=True,
                        help="云端频道默认启用邮箱注册。需要旧注册方式时使用 --no-use-email-registration。")
    args = parser.parse_args()
    try:
        if args.command in {"sign", "sign-full"}:
            sign(args)
            print("热更新清单已签名。")
        elif args.command == "export-files":
            export_files(args)
        else:
            configure(args)
            print("客户端公钥配置已生成。")
    except (OSError, ValueError, zipfile.BadZipFile) as error:
        parser.exit(1, f"{error}\n")


if __name__ == "__main__":
    main()
