$secure = Read-Host "Introduzca la contraseña que desea utilizar" -AsSecureString
$bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secure)
try {
    $plain = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr)
} finally {
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr)
}

$hash = [System.Security.Cryptography.SHA256]::Create().ComputeHash(
    [System.Text.Encoding]::UTF8.GetBytes($plain)
)
$hex = ([BitConverter]::ToString($hash) -replace '-', '').ToUpperInvariant()

$file = Join-Path $PSScriptRoot 'Program.cs'
$text = Get-Content $file -Raw
$text = $text.Replace('REPLACE_WITH_SHA256', $hex)
Set-Content -Path $file -Value $text -Encoding UTF8

dotnet publish (Join-Path $PSScriptRoot 'GestionarPapelera.csproj') `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -o (Join-Path $PSScriptRoot 'publish')

Write-Host ""
Write-Host "EXE creado en:"
Write-Host (Join-Path $PSScriptRoot 'publish\GestionarPapelera.exe')
