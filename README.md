# TaskManager Project

```text
/GestorTareas (Raíz del Repositorio)
├── 📂 .github/workflows
│   └── 📄 dotnet-ci.yml
├── 📂 .vscode
├── 📄 .gitignore
├── 📄 README.md
├── 📄 GestorTareas.sln
├── 🐳 docker-compose.yml
│
└── 📂 src (Código Fuente)
    ├── 📂 1. GestorTareas.Data
    │   ├── 📂 Configurations
    │   ├── 📂 Contexts
    │   ├── 📂 Entities
    │   ├── 📂 Interfaces
    │   ├── 📂 Repositories
    │   ├── 📂 Seeders
    │   └── 📂 Migrations
    │
    ├── 📂 2. GestorTareas.Business
    │   ├── 📂 Common
    │   ├── 📂 DTOs
    │   ├── 📂 Interfaces
    │   ├── 📂 Mappings
    │   ├── 📂 Services
    │   └── 📂 Validators
    │
    └── 📂 3. GestorTareas.API
        ├── 🐳 Dockerfile
        ├── 📂 Controllers
        ├── 📂 Extensions
        ├── 📂 Middlewares
        ├── 📄 appsettings.json
        └── 📄 Program.cs