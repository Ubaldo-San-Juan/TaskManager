# TaskManager Project
/GestorTareas (Raíz)
├── 📂 .github/workflows       # CI/CD (Automatización)
├── 📂 .vscode                 # Configuración del editor
├── 📄 .gitignore              # Archivos excluidos
├── 📄 README.md               # Documentación principal
├── 📄 GestorTareas.sln        # Solución de .NET
├── 🐳 docker-compose.yml      # SQL Server en contenedor
│
└── 📂 src/                    # CÓDIGO FUENTE
    │
    ├── 📂 1. GestorTareas.Data
    │   ├── 📄 Data.csproj
    │   ├── 📂 Configurations  # Fluent API
    │   ├── 📂 Contexts        # AppDbContext
    │   ├── 📂 Entities        # Modelos de BD (BaseEntity, Tarea)
    │   ├── 📂 Interfaces      # Contratos (ITareaRepository)
    │   ├── 📂 Repositories    # Implementación EF Core
    │   ├── 📂 Seeders         # Datos iniciales (Bogus)
    │   └── 📂 Migrations      # Historial de base de datos
    │
    ├── 📂 2. GestorTareas.Business
    │   ├── 📄 Business.csproj
    │   ├── 📂 Common          # ApiResponse, JwtSettings
    │   ├── 📂 DTOs            # Data Transfer Objects
    │   ├── 📂 Interfaces      # Contratos de servicios
    │   ├── 📂 Mappings        # AutoMapper Profiles
    │   ├── 📂 Services        # Lógica de negocio
    │   └── 📂 Validators      # FluentValidation
    │
    └── 📂 3. GestorTareas.API
        ├── 📄 API.csproj
        ├── 🐳 Dockerfile      # Imagen para despliegue
        ├── 📂 Controllers     # Endpoints REST
        ├── 📂 Extensions      # Inyección de dependencias
        ├── 📂 Middlewares     # Manejo global de errores
        ├── 📄 appsettings.json
        └── 📄 Program.cs      # Configuración de la App