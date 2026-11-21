"use strict";

const byId = (id) => document.getElementById(id);
const drop = byId("drop-zone");
const picker = byId("picker");
const choose = byId("choose");
const refresh = byId("refresh");
const filesList = byId("files");
const listStatus = byId("list-status");
const transfers = byId("transfers");
const transferLabel = byId("transfer-label");
const progress = byId("progress");
const failures = byId("failures");
const dateFormat = new Intl.DateTimeFormat("zh-CN", { dateStyle: "short", timeStyle: "short" });
const fileParams = (name) => new URLSearchParams({ name });
let maxSize = 4 * 1024 ** 3;
let uploading = false;

function formatSize(bytes) {
  const units = ["B", "KiB", "MiB", "GiB"];
  let unit = 0;
  while (bytes >= 1024 && unit < units.length - 1) {
    bytes /= 1024;
    unit += 1;
  }
  return new Intl.NumberFormat("zh-CN", { maximumFractionDigits: 1 }).format(bytes) + " " + units[unit];
}

function renderFile(file) {
  const row = document.createElement("li");
  row.className = "file";
  const name = document.createElement("span");
  name.className = "file-name";
  name.textContent = file.name;
  const meta = document.createElement("span");
  meta.className = "file-meta";
  meta.textContent = formatSize(file.size) + " · " + dateFormat.format(new Date(file.modified));
  const download = document.createElement("a");
  download.className = "download";
  download.href = "/download?" + fileParams(file.name);
  download.textContent = "下载";
  download.setAttribute("aria-label", "下载 " + file.name);
  row.append(name, meta, download);
  return row;
}

async function loadFiles() {
  refresh.disabled = true;
  try {
    const response = await fetch("/api/files");
    if (!response.ok) throw new Error("读取失败");
    const data = await response.json();
    maxSize = data.max;
    byId("capacity").textContent = formatSize(data.used) + " / " + formatSize(data.limit) + " 已用";
    byId("limit").textContent = "支持 APK、ZIP、存档等文件，单文件最大 " + formatSize(data.max);
    const mobileRelease = byId("mobile-release");
    mobileRelease.hidden = !data.publicVersion;
    mobileRelease.textContent = "手机端当前版本号 " + data.publicVersion + " · 上传更高版本的修真界 APK 会自动发布";
    filesList.replaceChildren(...data.files.map(renderFile));
    filesList.hidden = data.files.length === 0;
    listStatus.hidden = !filesList.hidden;
    listStatus.textContent = "上传的文件会显示在这里。";
  } catch {
    listStatus.hidden = false;
    listStatus.textContent = "读取列表失败，请刷新重试。";
    byId("capacity").textContent = "存储信息暂不可用";
  } finally {
    refresh.disabled = false;
  }
}

function uploadFile(file, onProgress) {
  return new Promise((resolve, reject) => {
    const request = new XMLHttpRequest();
    request.open("POST", "/api/files?" + fileParams(file.name));
    request.setRequestHeader("X-File-Vault", "1");
    request.upload.addEventListener("progress", (event) => {
      if (event.lengthComputable && event.total > 0) onProgress(event.loaded / event.total);
    });
    request.addEventListener("load", () => {
      if (request.status === 201) resolve(JSON.parse(request.responseText));
      else reject(new Error(request.responseText.trim() || "上传失败"));
    });
    request.addEventListener("error", () => reject(new Error("连接中断，请重试")));
    request.send(file);
  });
}

async function upload(files) {
  if (uploading || files.length === 0) return;
  uploading = true;
  choose.disabled = true;
  transfers.hidden = false;
  progress.hidden = false;
  progress.value = 0;
  failures.replaceChildren();
  failures.hidden = true;
  let completed = 0;
  let published = 0;
  for (const [index, file] of files.entries()) {
    try {
      if (file.size > maxSize) throw new Error("单文件超过 " + formatSize(maxSize));
      transferLabel.textContent = "上传中 " + (index + 1) + "/" + files.length + " · " + file.name;
      const result = await uploadFile(file, (fraction) => {
        progress.value = ((index + fraction) / files.length) * 100;
      });
      completed += 1;
      if (result.publishedVersion) published = result.publishedVersion;
      if (result.publishError) {
        const item = document.createElement("li");
        item.textContent = file.name + " · 已保存，发布到手机端失败";
        failures.append(item);
        failures.hidden = false;
      }
    } catch (error) {
      const item = document.createElement("li");
      item.textContent = file.name + " · " + error.message;
      failures.append(item);
      failures.hidden = false;
    }
  }
  transferLabel.textContent = "已上传 " + completed + " 个文件" +
    (completed === files.length ? "" : "，" + (files.length - completed) + " 个失败") +
    (published ? "，手机端已更新到版本号 " + published : "");
  progress.hidden = true;
  picker.value = "";
  choose.disabled = false;
  uploading = false;
  await loadFiles();
}

choose.addEventListener("click", () => picker.click());
picker.addEventListener("change", () => upload(Array.from(picker.files)));
refresh.addEventListener("click", loadFiles);
window.addEventListener("dragover", (event) => {
  if (event.dataTransfer.types.includes("Files")) event.preventDefault();
});
window.addEventListener("drop", (event) => {
  if (event.dataTransfer.files.length) event.preventDefault();
});
drop.addEventListener("dragover", (event) => {
  if (!event.dataTransfer.types.includes("Files")) return;
  event.preventDefault();
  event.dataTransfer.dropEffect = uploading ? "none" : "copy";
  if (!uploading) drop.classList.add("active");
});
drop.addEventListener("dragleave", (event) => {
  if (!drop.contains(event.relatedTarget)) drop.classList.remove("active");
});
drop.addEventListener("drop", (event) => {
  event.preventDefault();
  drop.classList.remove("active");
  upload(Array.from(event.dataTransfer.files));
});
loadFiles();
