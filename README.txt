# Gestionar Papelera - Windows 11

Pequeño lanzador para el usuario general.

Funcionamiento:
1. El usuario pulsa "Gestionar Papelera".
2. Se solicita una contraseña independiente de Windows.
3. Si es correcta, se abre la Papelera nativa mediante `shell:RecycleBinFolder`.

IMPORTANTE:
- Este programa NO sustituye la Papelera.
- No modifica `$Recycle.Bin`.
- No necesita permisos de administrador para abrir la Papelera del usuario actual.
- La contraseña se almacena como SHA-256, no en texto plano.
- Antes de compilar, sustituye `REPLACE_WITH_SHA256` en Program.cs por el SHA-256 de la contraseña elegida.

Compilación recomendada en un PC de administración:
- Visual Studio 2022 con ".NET desktop development", o
- .NET SDK compatible con Windows Forms.

El ejecutable resultante se puede copiar al PC de las enfermeras.
