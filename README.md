# 🏨 Hotel Reservas API

Sistema de gestión de reservas y citas para hoteles. Backend desarrollado en .NET 8 con Entity Framework Core y SQL Server en Azure.

## 📋 Tabla de Contenidos
- [Características](#características)
- [Tecnologías](#tecnologías)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Requisitos Previos](#requisitos-previos)
- [Configuración](#configuración)
- [Ejecución](#ejecución)
- [API Endpoints](#api-endpoints)
- [Pruebas](#pruebas)
- [Contacto](#contacto)

## ✨ Características

- ✅ Gestión completa de reservas de hotel (CRUD)
- ✅ Gestión de citas y servicios (CRUD)
- ✅ Validación de disponibilidad de habitaciones
- ✅ Base de datos SQL Server en Azure
- ✅ Documentación automática con Swagger
- ✅ Tests unitarios con xUnit
- ✅ Frontend en React (Vite)

## 🛠️ Tecnologías

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core 8** - ORM para base de datos
- **SQL Server** - Base de datos en Azure
- **Swagger/OpenAPI** - Documentación de API
- **xUnit** - Pruebas unitarias

### Frontend
- **React 18** - Biblioteca de UI
- **Vite** - Herramienta de build
- **Axios** - Cliente HTTP
- **React Router** - Navegación

## 📁 Estructura del Proyecto

```
Proyecto_Desarrollo/
├── backend/
│   ├── HotelReservasAPI/           # Proyecto principal
│   │   ├── Controllers/             # Controladores de la API
│   │   ├── Data/                    # Contexto de base de datos
│   │   ├── Models/                   # Modelos de datos
│   │   ├── Services/                 # Lógica de negocio
│   │   ├── Migrations/               # Migraciones de EF Core
│   │   └── Program.cs                 # Punto de entrada
│   ├── HotelReservasAPI.Tests/       # Proyecto de pruebas
│   │   ├── Controllers/               # Tests de controladores
│   │   ├── Services/                   # Tests de servicios
│   │   └── Helpers/                     # Utilidades para tests
│   └── HotelReservasAPI.sln            # Solución de Visual Studio
│
└── frontend/                           # Aplicación React
    ├── src/
    │   ├── components/                  # Componentes React
    │   ├── services/                     # Servicios de API
    │   └── App.jsx                        # Componente principal
    └── package.json                       # Dependencias
```

## 📋 Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)
- [Node.js 18+](https://nodejs.org/)
- [Git](https://git-scm.com/)

## ⚙️ Configuración

### 1. Clonar el repositorio
```bash
git clone https://github.com/Johan-molina/Proyecto_Desarrollo.git
cd Proyecto_Desarrollo
```

### 2. Configurar la base de datos

**Importante**: La contraseña de Azure SQL debe configurarse de forma segura:

```bash
cd backend/HotelReservasAPI
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=tcp:hotelreservas-serverr.database.windows.net,1433;Initial Catalog=HotelReservasDB;User ID=adminuser;Password=TU_CONTRASEÑA;Encrypt=True;"
```

### 3. Aplicar migraciones
```bash
cd backend/HotelReservasAPI
dotnet ef database update
```

### 4. Configurar el frontend
```bash
cd frontend
npm install
```

## 🚀 Ejecución

### Backend
```bash
cd backend/HotelReservasAPI
dotnet run
```
- API: `https://localhost:7191` o `http://localhost:5113`
- Swagger UI: `https://localhost:7191/swagger`

### Frontend
```bash
cd frontend
npm run dev
```
- App: `http://localhost:5173`

## 📚 API Endpoints

### Reservas
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Reservas` | Obtener todas las reservas |
| GET | `/api/Reservas/{id}` | Obtener reserva por ID |
| POST | `/api/Reservas` | Crear nueva reserva |
| PUT | `/api/Reservas/{id}` | Actualizar reserva |
| DELETE | `/api/Reservas/{id}` | Eliminar reserva |

### Citas
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Citas` | Obtener todas las citas |
| GET | `/api/Citas/{id}` | Obtener cita por ID |
| POST | `/api/Citas` | Crear nueva cita |
| PUT | `/api/Citas/{id}` | Actualizar cita |
| DELETE | `/api/Citas/{id}` | Eliminar cita |

### Ejemplo de creación de reserva
```json
{
  "nombreCliente": "Juan Pérez",
  "fechaEntrada": "2024-12-01T14:00:00",
  "fechaSalida": "2024-12-05T12:00:00",
  "numeroHabitacion": 101,
  "email": "juan@email.com",
  "telefono": "123456789"
}
```

## 🧪 Pruebas

### Ejecutar tests unitarios
```bash
cd backend/HotelReservasAPI.Tests
dotnet test
```

## 📧 Contacto

**Johan Molina** - [@Johan-molina](https://github.com/Johan-molina)

Link del proyecto: [https://github.com/Johan-molina/Proyecto_Desarrollo](https://github.com/Johan-molina/Proyecto_Desarrollo)

---

⭐️ **Si te gustó este proyecto, ¡no olvides darle una estrella en GitHub!**
