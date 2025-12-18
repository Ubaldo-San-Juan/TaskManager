# TaskManager Project
/GestorTareas (Raíz del Repositorio)
│
├── 📂 .github                  <-- CI/CD
│   └── 📂 workflows
│       └── 📄 dotnet-ci.yml    <-- Compilación automática en GitHub
│
├── 📂 .vscode                  <-- (Opcional) Configuración de workspace
├── 📄 .gitignore               <-- Ignora bin, obj, user secrets, appsettings.prod
├── 📄 README.md                <-- Documentación de setup para desarrolladores
├── 📄 GestorTareas.sln         <-- Archivo de Solución (.NET)
├── 🐳 docker-compose.yml       <-- Orquestador: Levanta SQL Server en contenedor
│
└── 📂 src                      <-- Código Fuente
    │
    ├── 📂 1. GestorTareas.Data  <-- CAPA DE PERSISTENCIA (Interactúa con BD)
    │   ├── 📄 GestorTareas.Data.csproj
    │   ├── 📂 Configurations    <-- (Fluent API) Configuración tablas y relaciones
    │   │   ├── 📄 TareaConfiguration.cs
    │   │   └── 📄 UsuarioConfiguration.cs
    │   ├── 📂 Contexts
    │   │   └── 📄 AppDbContext.cs   <-- DB Context (Sin lógica sucia, usa Configurations)
    │   ├── 📂 Entities          <-- Modelos de BD
    │   │   ├── 📄 BaseEntity.cs     <-- Id, CreatedAt, IsDeleted
    │   │   ├── 📄 Tarea.cs
    │   │   └── 📄 Usuario.cs
    │   ├── 📂 Interfaces        <-- Contratos de Repositorios
    │   │   └── 📄 ITareaRepository.cs
    │   ├── 📂 Repositories      <-- Implementación (EF Core)
    │   │   └── 📄 TareaRepository.cs
    │   ├── 📂 Seeders           <-- Datos iniciales / falsos (Bogus)
    │   │   └── 📄 DbSeeder.cs
    │   └── 📂 Migrations        <-- Historial de cambios SQL
    │
    │
    ├── 📂 2. GestorTareas.Business <-- CAPA DE NEGOCIO (Lógica Core)
    │   ├── 📄 GestorTareas.Business.csproj
    │   ├── 📂 Common            <-- Wrappers y Settings
    │   │   ├── 📄 ApiResponse.cs    <-- Respuesta estándar (Data, Message, Code)
    │   │   └── 📄 JwtSettings.cs
    │   ├── 📂 DTOs              <-- Data Transfer Objects
    │   │   ├── 📂 Auth          <-- Login/Register
    │   │   └── 📂 Tareas        <-- Crear/Listar
    │   ├── 📂 Interfaces        <-- Contratos de Servicios
    │   │   ├── 📄 IAuthService.cs
    │   │   └── 📄 ITareaService.cs
    │   ├── 📂 Mappings          <-- AutoMapper Profiles
    │   │   └── 📄 AutoMapperProfile.cs
    │   ├── 📂 Services          <-- Lógica e implementación
    │   │   ├── 📄 AuthService.cs
    │   │   └── 📄 TareaService.cs
    │   └── 📂 Validators        <-- FluentValidation (Reglas de negocio)
    │       └── 📄 CrearTareaValidator.cs
    │
    │
    └── 📂 3. GestorTareas.API       <-- CAPA DE PRESENTACIÓN (Entrada)
        ├── 📄 GestorTareas.API.csproj
        ├── 🐳 Dockerfile               <-- Imagen Docker para despliegue de la API
        ├── 📂 Controllers            <-- Endpoints HTTP (REST)
        │   ├── 📄 AuthController.cs
        │   └── 📄 TareasController.cs
        ├── 📂 Extensions             <-- Inyección de Dependencias limpia
        │   ├── 📄 ApplicationServiceExtensions.cs
        │   └── 📄 IdentityServiceExtensions.cs
        ├── 📂 Middlewares             <-- Manejo Global de Errores
        │   └── 📄 ErrorHandlerMiddleware.cs
        ├── 📄 appsettings.json             <-- Config Genérica
        ├── 📄 appsettings.Development.json <-- Config Local (BD Docker Local)
        ├── 📄 appsettings.Staging.json     <-- Config QA
        └── 📄 Program.cs                   <-- Punto de entrada (Configuración mínima)