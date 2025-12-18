# TaskManager Project

```text
/TaskManager (Root Path)
├── 📂 .github/workflows
│   └── 📄 dotnet-ci.yml
├── 📂 .vscode
├── 📄 .gitignore
├── 📄 README.md
├── 📄 TaskManager.sln
├── 🐳 docker-compose.yml
│
└── 📂 src
    ├── 📂 1. TaskManager.Data
    │   ├── 📂 Configurations
    │   ├── 📂 Contexts
    │   ├── 📂 Entities
    │   ├── 📂 Interfaces
    │   ├── 📂 Repositories
    │   ├── 📂 Seeders
    │   └── 📂 Migrations
    │
    ├── 📂 2. TaskManager.Business
    │   ├── 📂 Common
    │   ├── 📂 DTOs
    │   ├── 📂 Interfaces
    │   ├── 📂 Mappings
    │   ├── 📂 Services
    │   └── 📂 Validators
    │
    └── 📂 3. TaskManager.API
        ├── 🐳 Dockerfile
        ├── 📂 Controllers
        ├── 📂 Extensions
        ├── 📂 Middlewares
        ├── 📄 appsettings.json
        └── 📄 Program.cs