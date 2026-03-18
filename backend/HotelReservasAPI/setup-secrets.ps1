# setup-secrets.ps1
Write-Host "🔧 Configurando User Secrets..." -ForegroundColor Yellow

# Inicializar User Secrets
Write-Host "1. Inicializando User Secrets..." -ForegroundColor Cyan
dotnet user-secrets init

# Solicitar contraseña de forma segura
$password = Read-Host "`n🔑 Ingresa la contraseña de Azure SQL para el usuario 'adminuser'" -AsSecureString
$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($password)
$plainPassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)

# Construir cadena de conexión
$connectionString = "Server=tcp:hotelreservas-server.database.windows.net,1433;Initial Catalog=HotelReservasDB;User ID=adminuser;Password=$plainPassword;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

# Guardar en User Secrets
Write-Host "2. Guardando cadena de conexión..." -ForegroundColor Cyan
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "$connectionString"

# Verificar
Write-Host "3. Verificando secretos guardados..." -ForegroundColor Cyan
dotnet user-secrets list

Write-Host "`n✅ User Secrets configurado correctamente!" -ForegroundColor Green
Write-Host "Ejecuta 'dotnet run' para probar la conexión." -ForegroundColor Yellow

pause