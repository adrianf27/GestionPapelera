param(
    [Parameter(Mandatory=$true)]
    [string]$Password
)

$hash = [System.Security.Cryptography.SHA256]::Create().ComputeHash(
    [System.Text.Encoding]::UTF8.GetBytes($Password)
)
$hex = ([BitConverter]::ToString($hash) -replace '-', '').ToUpperInvariant()

$file = Join-Path $PSScriptRoot 'Program.cs'
$text = Get-Content $file -Raw
$text = $text.Replace('REPLACE_WITH_SHA256', $hex)
Set-Content -Path $file -Value $text -Encoding UTF8

Write-Host "SHA-256 configurado: $hex"
Write-Host "Ahora compila el proyecto."
