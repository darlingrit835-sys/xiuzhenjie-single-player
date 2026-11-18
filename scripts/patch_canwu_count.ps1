param(
  [string]$SrcApk = 'client/修真界单机版.apk',
  [string]$OutApk = 'out/修真界单机版.apk',
  [switch]$DryRun,
  [string]$Cecil = 'cecil/lib/netstandard2.0/Mono.Cecil.dll',
  [string]$ZipAlign = 'android/zipalign.exe',
  [string]$ApkSigner = 'android/apksigner.bat',
  [string]$JavaHome = 'jdk8',
  [string]$Keystore = 'sign.keystore',
  [string]$KsPass = 'xzj123456',
  [string]$KsAlias = 'xzj'
)

# 参悟页顶部“已学习书籍 N 本”修正补丁
#
# 背景：客户端 CanWuWnd.EnterCanWu 的回包里，它是这么算的：
#     m_HaveLearnBooks = openUnderstandPanelResp_.Object.Count;
# 也就是直接把“列表条目数”当成已学数。可参悟的书必须一起下发（否则没东西可点），
# 所以这个数字恒等于功法总数，服务端怎么改数据都改不回来。
#
# 做法：把 IL 里的 get_Object()/get_Count() 换成 get_Code()，由服务端把真实已学数
# 放在 Code 字段里（CanWuListHandler 中 b.setCode(learned)）。

$ErrorActionPreference = 'Stop'
[void][System.Reflection.Assembly]::LoadFrom($Cecil)
$tmp = Join-Path $env:TEMP ('xzj_canwu_' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Force -Path $tmp | Out-Null
$outDir = Split-Path -Parent $OutApk
if ($outDir -and -not (Test-Path $outDir)) { New-Item -ItemType Directory -Force -Path $outDir | Out-Null }

$managedDir = Join-Path $tmp 'managed'
New-Item -ItemType Directory -Force -Path $managedDir | Out-Null
$zr = [System.IO.Compression.ZipFile]::OpenRead($SrcApk)
$prefix = 'assets/bin/Data/Managed/'
foreach ($en in $zr.Entries) {
  if ($en.FullName -notlike "$prefix*") { continue }
  if ([string]::IsNullOrEmpty($en.Name)) { continue }
  $rel = $en.FullName.Substring($prefix.Length)
  $dest = Join-Path $managedDir $rel
  $subDir = Split-Path -Parent $dest
  if (-not (Test-Path -LiteralPath $subDir)) { New-Item -ItemType Directory -Force -Path $subDir | Out-Null }
  $outS = [IO.File]::Create($dest)
  $inS = $en.Open(); $inS.CopyTo($outS); $inS.Dispose(); $outS.Dispose()
}
$zr.Dispose()
$p1 = Join-Path $managedDir 'Assembly-CSharp.dll'
if (-not (Test-Path $p1)) { throw '未提取到 Assembly-CSharp.dll' }

function Patch-Asm([string]$path, [scriptblock]$mutate) {
  $rp = New-Object Mono.Cecil.ReaderParameters
  $rp.ReadingMode = [Mono.Cecil.ReadingMode]::Immediate
  $resolver = New-Object Mono.Cecil.DefaultAssemblyResolver
  $resolver.AddSearchDirectory((Split-Path -Parent $path))
  $rp.AssemblyResolver = $resolver
  $asm = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($path, $rp)
  $changed = & $mutate $asm
  if (-not $DryRun) {
    $tmpOut = "$path.tmp"
    $asm.Write($tmpOut); $asm.Dispose()
    Move-Item -Force $tmpOut $path
  } else { $asm.Dispose() }
  $changed
}

$nChanged = Patch-Asm $p1 {
  param($asm)
  $proto = $asm.MainModule.GetType('OpenUnderstandPanelResp_1001011')
  if (-not $proto) {
    foreach ($t in $asm.Modules[0].Types) { if ($t.Name -eq 'OpenUnderstandPanelResp_1001011') { $proto = $t; break } }
  }
  if (-not $proto) { throw '找不到 OpenUnderstandPanelResp_1001011 类型' }
  $getCode = $proto.Methods | Where-Object { $_.Name -eq 'get_Code' -and $_.Parameters.Count -eq 0 }
  if (-not $getCode) { throw '找不到 get_Code' }
  Write-Host ("  get_Code 返回类型: {0}" -f $getCode.ReturnType)

  $script:count = 0
  function Walk($types) {
    foreach ($t in $types) {
      foreach ($m in $t.Methods) {
        if (-not $m.HasBody) { continue }
        $ins = $m.Body.Instructions
        for ($i = 0; $i + 2 -lt $ins.Count; $i++) {
          if ($ins[$i].OpCode.Name -ne 'callvirt') { continue }
          if (([string]$ins[$i].Operand) -notmatch 'get_Object\(\)') { continue }
          if ($ins[$i+1].OpCode.Name -ne 'callvirt') { continue }
          if (([string]$ins[$i+1].Operand) -notmatch 'get_Count\(\)') { continue }
          if ($ins[$i+2].OpCode.Name -ne 'stfld') { continue }
          if (([string]$ins[$i+2].Operand) -notmatch 'm_HaveLearnBooks') { continue }

          Write-Host ("  命中 {0}.{1} @{2}: get_Object/get_Count -> get_Code" -f $t.FullName, $m.Name, $i)
          $ins[$i].Operand = $getCode
          [void]$ins.Remove($ins[$i+1])
          $script:count = $script:count + 1
        }
      }
      if ($t.NestedTypes.Count -gt 0) { Walk $t.NestedTypes }
    }
  }
  [void](Walk $asm.Modules[0].Types)
  [int]$script:count
}
Write-Host ("补丁点: {0} 处" -f $nChanged)
if ([int]$nChanged -ne 1) { throw "补丁点数量异常: $nChanged（期望 1 处）" }

if ($DryRun) { Write-Host 'DryRun：未写回、未打包'; exit 0 }

$m1 = 'assets/bin/Data/Managed/Assembly-CSharp.dll'
Copy-Item -Force $SrcApk $OutApk
$zip = [System.IO.Compression.ZipFile]::Open($OutApk, 'Update')
foreach ($e in @($zip.Entries)) {
  if ($e.FullName -match '^META-INF/' -and $e.FullName -match '\.(SF|MF|RSA|DSA|EC)$') { $e.Delete() }
}
$e = $zip.GetEntry($m1); if ($e) { $e.Delete() }
$entry = $zip.CreateEntry($m1, [System.IO.Compression.CompressionLevel]::Optimal)
$s = $entry.Open(); $fs = [IO.File]::OpenRead($p1); $fs.CopyTo($s); $fs.Dispose(); $s.Dispose()
$zip.Dispose()
Write-Host 'APK 内容已替换'

$aligned = Join-Path $tmp 'aligned.apk'
& $ZipAlign -f -p 4 $OutApk $aligned | Out-Null
if ($LASTEXITCODE -ne 0) { throw "zipalign 失败 ($LASTEXITCODE)" }
$env:JAVA_HOME = $JavaHome
$env:PATH = "$JavaHome\bin;$env:PATH"
& $ApkSigner sign --ks $Keystore --ks-pass "pass:$KsPass" --ks-key-alias $KsAlias --key-pass "pass:$KsPass" --out $OutApk $aligned 2>&1 | Out-Host
if ($LASTEXITCODE -ne 0) { throw "apksigner 失败 ($LASTEXITCODE)" }
& $ApkSigner verify --verbose $OutApk 2>&1 | Select-Object -First 6 | Out-Host
Remove-Item -Recurse -Force $tmp
Write-Host ("完成: {0}  ({1:N1} MB)" -f $OutApk, ((Get-Item $OutApk).Length / 1MB))
