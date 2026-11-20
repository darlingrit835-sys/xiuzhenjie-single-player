"""从项目原图与本地画稿重建 256px 图鉴图标。需 Pillow、Node 和 sharp。"""

import argparse
import io
import math
from pathlib import Path
import random
import re
import subprocess
from uuid import NAMESPACE_URL, uuid5
import xml.etree.ElementTree as ET

from PIL import Image, ImageChops, ImageDraw, ImageFilter, ImageOps

ROOT = Path(__file__).resolve().parents[2]
ASSETS = ROOT / "UnityProject/Assets"
ART = ROOT / "art/catalog"
RESOURCES = ASSETS / "Resources"
OUT = RESOURCES / "CatalogIcons/Monsters"
SVG = "{http://www.w3.org/2000/svg}"
ET.register_namespace("", SVG[1:-1])
ET.register_namespace("xlink", "http://www.w3.org/1999/xlink")
P = Image.Resampling.LANCZOS

# 主色取自物种而非所属郡。数字是现有 monstericon 图像键，非旧服发送的图标槽位。
STUDIES = {
    101: (39, "flame"), 102: (6, "tide"), 103: (27, "leaf"), 104: (17, "earth"),
    105: (20, "gold"), 106: (10, "moon"), 107: (1, "thunder"), 108: (9, "jade"),
    109: (21, "marsh"), 201: ("Man_2", "earth"), 202: ("Women_3", "moon"),
    203: ("Man_5", "flame"), 204: ("Women_6", "leaf"), 205: ("Man_1", "steel"),
    206: ("Man_3", "gold"), 207: ("Women_4", "jade"), 208: ("Women_5", "earth"),
    301: ("mantis", "leaf"), 302: ("study", "earth"), 303: (14, "jade"),
    304: ("study", "steel"), 305: ("study", "tide"), 306: ("study", "leaf"),
    307: ("study", "marsh"), 308: (31, "gold"), 309: ("study", "gold"),
    310: ("study", "moon"), 311: (11, "mist"), 312: ("study", "marsh"),
    313: ("study", "flame"), 314: (16, "cinnabar"), 315: (4, "cinnabar"),
    316: (2, "flame"), 317: ("study", "earth"), 318: (25, "ink"),
    319: ("study", "earth"), 320: ("study", "earth"), 321: (38, "mist"),
    322: ("study", "tide"), 323: ("study", "steel"), 324: (8, "mist"),
}
# 墨、设色、纸白。原模型与项目水墨人物分别保留少量固有色。
COLORS = {
    "flame": ("#322820", "#ad6143", "#f6d7a2"), "cinnabar": ("#31201e", "#8e4a3c", "#efd1ac"),
    "tide": ("#243631", "#618a87", "#e0dfbf"), "marsh": ("#28382e", "#71836b", "#dfd3a9"),
    "leaf": ("#263a2d", "#6f9270", "#dfdfb0"), "earth": ("#352e25", "#978059", "#e7d7b2"),
    "gold": ("#3a3027", "#b2975c", "#f2db99"), "moon": ("#343042", "#8c819d", "#e3d9c4"),
    "thunder": ("#372f32", "#a48961", "#f2d7a0"), "jade": ("#2a3d37", "#87a391", "#e8e8ce"),
    "steel": ("#2b393e", "#819394", "#e5dfc4"), "mist": ("#343e43", "#9daeb0", "#f1edd6"),
    "ink": ("#242c2c", "#536163", "#aaa997"),
}


def load_study(root, key, node):
    """一次只渲染该物种的 SVG 图组。保持图组引用的颜色及内部重复的左右翼。"""
    sample = ET.Element(SVG + "svg", viewBox="0 0 200 200", width="500", height="500")
    sample.append(root.find(SVG + "defs"))
    group = root.find(f"{SVG}g[@id='m{key}']")
    if group is None:
        raise ValueError(f"画稿缺少 m{key}")
    sample.append(group)
    script = ("const sharp=require('sharp'),fs=require('fs');"
              "sharp(fs.readFileSync(0)).png().toBuffer().then(b=>process.stdout.write(b))"
              ".catch(e=>{console.error(e);process.exitCode=1;});")
    result = subprocess.run([node, "-e", script], input=ET.tostring(sample),
                            check=True, stdout=subprocess.PIPE)
    return Image.open(io.BytesIO(result.stdout)).convert("RGBA")


def wash_mask(size, key):
    # 无固定描边的笔圈，边缘粗细起伏，给透明 UI 底色留出呼吸空间。
    rng = random.Random(key)
    phase = rng.uniform(0, 2 * math.pi)
    points = []
    for angle in range(0, 360, 2):
        rad = angle * math.pi / 180
        radius = 104 + 3.5 * math.sin(rad*3 + phase) + 3*math.sin(rad*5 - phase) + rng.uniform(-1.8, 1.8)
        points.append((size/2 + radius*math.cos(rad), size/2 + .95*radius*math.sin(rad)))
    mask = Image.new("L", (size, size))
    ImageDraw.Draw(mask).polygon(points, fill=165)
    return mask.filter(ImageFilter.GaussianBlur(1.1))


def vignette(key, asset, tone, studies, node):
    rng = random.Random(key)
    dark, middle, light = COLORS[tone]
    portrait = isinstance(asset, str) and "_" in asset
    if portrait:
        subject = Image.open(RESOURCES / "InkPortraits" / (asset + ".png")).convert("RGBA")
    elif asset == "mantis":
        subject = Image.open(ART / "qinglan-mantis.png").convert("RGBA")
    elif asset == "study":
        subject = load_study(studies, key, node)
    else:
        subject = Image.open(ASSETS / "gamedata/textures/monstericon" / (str(asset) + ".png")).convert("RGBA")
    src_alpha = subject.getchannel("A")
    if isinstance(asset, int):
        # 旧图为截取全身动作的 100px 卡面，在切边处收墨以保留透明头像轮廓。
        w,h = subject.size
        edge = Image.new("L", subject.size, 255)
        draw = ImageDraw.Draw(edge)
        for gap, opacity in enumerate((15, 57, 116, 177, 222)):
            draw.rectangle((gap,gap,w-gap-1,h-gap-1), outline=opacity)
        src_alpha = ImageChops.multiply(src_alpha, edge)
    gray = ImageOps.grayscale(subject)
    # 保留亮部高光和墨线；灰图与原固有色混合，避免原模型高饱和塑胶光泽。
    ink = ImageOps.colorize(gray, dark, light, middle, midpoint=132)
    base = Image.blend(ink, subject.convert("RGB"), .46 if portrait or asset in ("study","mantis") else .19)
    if asset == "study":
        # 手绘色面上叠细宣纸纤维，保留墨线边缘。
        texture = Image.frombytes("L", base.size, rng.randbytes(base.width * base.height))
        fiber = ImageOps.colorize(texture, "#75816b", "#ece0bd")
        base = Image.blend(base, fiber, .095)
    subject = base.convert("RGBA")
    if portrait:
        # 人物方形原画原有的宣纸底在画幅边缘淡出。
        side = 218
        subject = subject.resize((side, side), P)
        alpha = Image.new("L", (side, side), 0)
        ImageDraw.Draw(alpha).ellipse((7, 1, side-4, side-2), fill=255)
        subject.putalpha(alpha.filter(ImageFilter.GaussianBlur(5)))
    else:
        subject.putalpha(src_alpha)
        box = src_alpha.point(lambda a: 255 if a >= 11 else 0).getbbox()
        subject = subject.crop(box)
        ratio = min(222/subject.width, 213/subject.height)
        subject = subject.resize((round(subject.width*ratio), round(subject.height*ratio)), P)

    mask = wash_mask(256, key)
    glow = Image.new("RGBA", (256, 256), light)
    glow.putalpha(mask)
    texture = Image.new("RGBA", glow.size)
    brush = ImageDraw.Draw(texture)
    for j in range(95):
        x, y = rng.randrange(25,230), rng.randrange(25,229)
        if mask.getpixel((x,y)) > 110:
            brush.line([(x,y), (x+rng.randrange(1,4), y-1)], fill=(101,83,52,rng.randrange(8,29)), width=1)
    # 三笔断开的淡金和墨弧，保持每张卡的共同笔触。
    brush.arc((19, 26, 236, 232), 144, 222, fill=(88,75,50,100), width=2)
    brush.arc((23, 29, 232, 230), 148, 200, fill=(117,88,57,72), width=1)
    brush.arc((21, 23, 236, 230), 299, 357, fill=(125,107,67,84), width=2)
    glow.alpha_composite(texture)
    x,y = (256-subject.width)//2, (256-subject.height)//2
    glow.alpha_composite(subject, (x,y))
    if portrait:
        glow.alpha_composite(insignia(key), (166, 168))
    return glow


def insignia(key):
    """人形对手以随身器物区分行当，保留原水墨肖像。"""
    seal = Image.new("RGBA", (80,80))
    pen = ImageDraw.Draw(seal)
    edge, pale, ochre = "#544a39", "#e5dabb", "#a88a5f"
    pen.ellipse((6,6,73,73), fill=(236,226,197,240), outline=(111,91,57,200), width=2)
    def line(points, width=2):
        pen.line(points, fill=edge, width=width, joint="curve")
    def shape(points, color):
        pen.polygon(points, fill=color, outline=edge, width=2)
    if key == 201:  # 竹简小卷
        shape([(20,31),(51,20),(57,45),(26,58)], pale)
        line([(21,31),(26,57)],4); line([(52,20),(58,45)],4)
        for dx in (0,5,10):
            line([(29+dx,34-dx//3),(32+dx,49-dx//3)],1)
    elif key == 202:  # 邪修骨面
        shape([(23,24),(32,31),(41,27),(52,30),(58,20),(54,46),(40,59),(26,46)],ochre)
        shape([(29,36),(39,40),(33,43)],edge);shape([(43,40),(53,35),(49,43)],edge)
        line([(38,50),(44,48)])
    elif key == 203:  # 焰纹魔珠
        shape([(22,48),(27,35),(35,31),(35,19),(45,28),(46,38),(55,28),(58,46),(51,55),(35,59)],"#9d654c")
        pen.ellipse((32,40,51,55),fill=edge);line([(35,45),(44,43)],2)
    elif key == 204:  # 灵兽爪
        for x,y in ((24,29),(34,22),(46,22),(55,30)):
            pen.ellipse((x-4,y-2,x+3,y+9), fill=ochre, outline=edge,width=2)
        shape([(26,47),(33,40),(38,36),(43,37),(51,44),(56,51),(52,55),(40,52),(30,56)],ochre)
    elif key == 205:  # 直脊长剑
        shape([(22,56),(29,44),(53,19),(59,16),(57,25),(35,50)],"#9aaba3")
        line([(25,42),(38,53)],3);line([(28,51),(19,61)],4);line([(34,45),(53,23)],1)
    elif key == 206:  # 宽刃弯刀
        shape([(22,53),(30,42),(40,34),(51,25),(57,17),(59,27),(54,40),(36,53)],"#b0a482")
        line([(25,43),(37,54)],3);line([(28,50),(20,60)],4)
    elif key == 207:  # 阵盘
        pen.ellipse((19,20,61,61),outline=edge,width=3);pen.ellipse((25,26,55,55),outline=ochre,width=2)
        line([(40,23),(54,48),(25,48),(40,23)]);line([(39,58),(25,31),(54,31),(39,58)],1)
    elif key == 208:  # 丹炉
        pen.ellipse((22,33,57,55),fill=ochre,outline=edge,width=2)
        pen.ellipse((20,31,59,40),fill=pale,outline=edge,width=2)
        line([(25,39),(17,34),(17,44),(23,45)]);line([(56,39),(63,34),(64,44),(58,45)])
        line([(28,50),(25,58)],3);line([(50,50),(54,57)],3)
        line([(29,31),(34,27),(47,27),(52,32)]);pen.ellipse((37,22,43,28),fill=ochre,outline=edge)
    return seal.resize((64,64), P)


def meta(path, template, identity):
    value = uuid5(NAMESPACE_URL, "xiuzhenjie/catalogIcons/Monsters/" + identity).hex
    expected = re.sub(r"(?m)^guid: [0-9a-f]+$", "guid: " + value, template)
    if not path.exists():
        path.write_text(expected, encoding="utf-8")


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--node", default="node", help="装有 sharp 模块的 Node 运行时")
    args = parser.parse_args()
    studies = ET.parse(ART / "monster-studies.svg").getroot()
    OUT.mkdir(parents=True, exist_ok=True)
    folder = (RESOURCES / "InkInterface.meta").read_text()
    sprite = (RESOURCES / "InkInterface/Book.png.meta").read_text()
    meta(OUT.with_suffix(".meta"), folder, "folder")
    for key, (asset, tone) in STUDIES.items():
        target = OUT / f"{key}.png"
        vignette(key, asset, tone, studies, args.node).save(target, optimize=True)
        meta(target.with_suffix(".png.meta"), sprite, str(key))


if __name__ == "__main__":
    main()
