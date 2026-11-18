# 客户端改包：把 APK 里的服务器地址改成 127.0.0.1，重打包并重签名
# 依赖：Mono.Cecil（改 DLL）、zipalign、apksigner（Android build-tools）、JDK
# 用法：powershell -File patch_apk.ps1 -SrcApk 原包.apk -OutApk 输出.apk -Cecil cecil路径 -ZipAlign .. -ApkSigner .. -Keystore ..
param(
  [string]$SrcApk = '原包.apk',
  [string]$OutApk = '修真界单机版.apk',
  [string]$Ip   = '127.0.0.1',
  [int]$Port    = 9443,
  [string]$Cecil = 'cecil\lib\netstandard2.0\Mono.Cecil.dll',
  [string]$ZipAlign = 'zipalign.exe',
  [string]$ApkSigner = 'apksigner.bat',
  [string]$JavaHome = '',
  [string]$Keystore = 'sign.keystore',
  [string]$KsPass = 'xzj123456',
  [string]$KsAlias = 'xzj'
)

$ErrorActionPreference = 'Stop'
[void][System.Reflection.Assembly]::LoadFrom($Cecil)
$tmp = Join-Path $env:TEMP ('xzj_patch_' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Force -Path $tmp | Out-Null
New-Item -ItemType Directory -Force -Path (Split-Path -Parent $OutApk) | Out-Null

# 1) 取出整个 Managed 目录（Cecil 写回时需要解析依赖）
$m1 = 'assets/bin/Data/Managed/Assembly-CSharp.dll'
$m2 = 'assets/bin/Data/Managed/Resource.dll'
$managedDir = Join-Path $tmp 'managed'
New-Item -ItemType Directory -Force -Path $managedDir | Out-Null
& tar.exe -xf $SrcApk -C $managedDir 'assets/bin/Data/Managed'
Move-Item -Force (Join-Path $managedDir 'assets\bin\Data\Managed\*') $managedDir
Remove-Item -Recurse -Force (Join-Path $managedDir 'assets')
$p1 = Join-Path $managedDir 'Assembly-CSharp.dll'
$p2 = Join-Path $managedDir 'Resource.dll'
if (-not (Test-Path $p1)) { throw '未提取到 Assembly-CSharp.dll' }

# 2) Cecil 改地址
function Patch-Asm([string]$path, [scriptblock]$mutate) {
  $rp = New-Object Mono.Cecil.ReaderParameters
  $rp.ReadingMode = [Mono.Cecil.ReadingMode]::Immediate
  $resolver = New-Object Mono.Cecil.DefaultAssemblyResolver
  $resolver.AddSearchDirectory((Split-Path -Parent $path))
  $rp.AssemblyResolver = $resolver
  $asm = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($path, $rp)
  $changed = & $mutate $asm
  $tmpOut = "$path.tmp"
  $asm.Write($tmpOut); $asm.Dispose()
  Move-Item -Force $tmpOut $path
  $changed
}

$ipChanged = Patch-Asm $p1 {
  param($asm)
  $n = 0
  function Walk($types) {
    foreach ($t in $types) {
      foreach ($m in $t.Methods) {
        if (-not $m.HasBody) { continue }
        $ins = $m.Body.Instructions
        for ($i = 0; $i -lt $ins.Count; $i++) {
          if ($ins[$i].OpCode.Name -eq 'ldstr' -and ([string]$ins[$i].Operand) -match '^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$') {
            if ($i + 1 -lt $ins.Count -and ([string]$ins[$i+1].Operand) -match 'LoginServerIP') {
              Write-Host ("  IP  {0} -> {1}  ({2})" -f $ins[$i].Operand, $Ip, $t.FullName)
              $script:n++
              $ins[$i].Operand = $Ip
            }
          }
        }
      }
      if ($t.NestedTypes.Count -gt 0) { Walk $t.NestedTypes }
    }
  }
  Walk $asm.Modules[0].Types
  foreach ($m in $asm.Modules[0].GetType('XiuZhenJie.Constants').Methods) {
    if ($m.Name -ne '.cctor' -or -not $m.HasBody) { continue }
    $ins = $m.Body.Instructions
    for ($i = 0; $i -lt $ins.Count - 1; $i++) {
      if (($ins[$i].OpCode.Name -eq 'ldc.i4' -or $ins[$i].OpCode.Name -eq 'ldc.i4.s') -and
          ([string]$ins[$i+1].Operand) -match 'LoginServerPort') {
        $ins[$i].Operand = [int]$Port
        Write-Host ("  PORT -> {0}" -f $Port)
        $script:n++
      }
    }
  }
  $script:n
}

$urlChanged = Patch-Asm $p2 {
  param($asm)
  $n = 0
  foreach ($mod in $asm.Modules) {
    foreach ($t in $mod.Types) {
      foreach ($m in $t.Methods) {
        if (-not $m.HasBody) { continue }
        foreach ($ins in $m.Body.Instructions) {
          if ($ins.OpCode.Name -eq 'ldstr' -and ([string]$ins.Operand) -match '10\.0\.2\.2|127\.0\.0\.1') {
            $old = [string]$ins.Operand
            $new = [regex]::Replace($old, '(?<=//)\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}(?=:)', $Ip)
            $new = [regex]::Replace($new, "(?<=//)(\d{1,3}\.){3}\d{1,3}", $Ip)
            if ($new -ne $old) { Write-Host ("  URL {0} -> {1}" -f $old, $new); $script:n++; $ins.Operand = $new }
          }
        }
      }
    }
  }
  $script:n
}
Write-Host "DLL 补丁: Assembly-CSharp=$ipChanged 处, Resource=$urlChanged 处"

# 3) 写回 APK（删掉旧签名）
Copy-Item -Force $SrcApk $OutApk
$zip = [System.IO.Compression.ZipFile]::Open($OutApk, 'Update')
foreach ($e in @($zip.Entries)) {
  if ($e.FullName -match '^META-INF/' -and $e.FullName -match '\.(SF|MF|RSA|DSA|EC)$') { $e.Delete() }
}
function Replace-Entry($zip, $name, $file) {
  $e = $zip.GetEntry($name); if ($e) { $e.Delete() }
  $entry = $zip.CreateEntry($name, [System.IO.Compression.CompressionLevel]::Optimal)
  $s = $entry.Open(); $fs = [IO.File]::OpenRead($file); $fs.CopyTo($s); $fs.Dispose(); $s.Dispose()
}
Replace-Entry $zip $m1 $p1
Replace-Entry $zip $m2 $p2
$zip.Dispose()
Write-Host 'APK 内容已替换'

# 4) zipalign + 签名
$aligned = Join-Path $tmp 'aligned.apk'
& $ZipAlign -f -p 4 $OutApk $aligned | Out-Null
if ($LASTEXITCODE -ne 0) { throw "zipalign 失败 ($LASTEXITCODE)" }
if ($JavaHome) { $env:JAVA_HOME = $JavaHome; $env:PATH = "$JavaHome\bin;$env:PATH" }
& $ApkSigner sign --ks $Keystore --ks-pass "pass:$KsPass" --ks-key-alias $KsAlias --key-pass "pass:$KsPass" --out $OutApk $aligned 2>&1 | Out-Host
if ($LASTEXITCODE -ne 0) { throw "apksigner 失败 ($LASTEXITCODE)" }
& $ApkSigner verify --verbose $OutApk 2>&1 | Select-Object -First 8 | Out-Host
Remove-Item -Recurse -Force $tmp
Write-Host ("完成: {0}  ({1:N1} MB)" -f $OutApk, ((Get-Item $OutApk).Length / 1MB))
