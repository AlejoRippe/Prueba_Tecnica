🐱 Cat Facts & Giphy App
Una aplicación web que combina datos curiosos sobre gatos con GIFs relacionados, desarrollada con ASP.NET Core (backend) y Angular (frontend).
📋 Características

✅ Obtención de facts aleatorios sobre gatos desde API externa
✅ GIFs relacionados obtenidos automáticamente desde Giphy API
✅ Historial persistente de todas las búsquedas realizadas
✅ Interfaz moderna con diseño responsive
✅ Guardado automático en base de datos local
✅ Estadísticas de uso personal

🏗️ Arquitectura
Backend (ASP.NET Core)

Framework: ASP.NET Core Web API
Base de Datos: SQLite con Entity Framework Core
APIs Externas: Cat Facts API + Giphy API
Patrones: Repository Pattern, Dependency Injection

Frontend (Angular)

Framework: Angular con TypeScript
Estilos: CSS personalizado con diseño moderno
HTTP Client: Para comunicación con backend
UI/UX: Interfaz responsive con efectos visuales

🚀 Instalación y Configuración
Pre-requisitos

Backend:

.NET 8.0 SDK
Visual Studio 2022 o VS Code


Frontend:

Node.js 16+
Angular CLI: npm install -g @angular/cli



🔧 Configuración del Backend

Navegar al directorio del backend:
cd Backend/CatFactsGiphyAPI

Restaurar dependencias:
dotnet restore

Crear base de datos:
dotnet ef database update
O la base de datos se creará automáticamente al ejecutar.
Ejecutar backend:
dotnet run
✅ Verificar: El backend estará disponible en http://localhost:5000

🌐 Configuración del Frontend

Navegar al directorio del frontend:
cd Frontend/cat-facts-app

Instalar dependencias:
npm install

Ejecutar frontend:
ng serve
✅ Verificar: El frontend estará disponible en http://localhost:4200

🎮 Uso de la Aplicación
Funcionalidades Principales

🎲 Generar Fact:

Click en "Obtener Nuevo Fact"
Se obtiene un dato curioso sobre gatos
Se muestra un GIF relacionado automáticamente
Se guarda automáticamente en el historial


🔄 Cambiar GIF:

Click en "Cambiar GIF"
Mantiene el mismo fact pero busca un GIF diferente
Utiliza sistema de offset para variedad


📚 Ver Historial:

Click en "Ver Historial"
Muestra todos los facts generados anteriormente
Ordenados por fecha (más recientes primero)
Click en cualquier elemento para verlo de nuevo


🔗 APIs Utilizadas
APIs Externas

Cat Facts API: https://catfact.ninja/fact

Proporciona datos curiosos sobre gatos
No requiere autenticación


Giphy API: https://api.giphy.com/v1/gifs/search

Proporciona GIFs relacionados con las palabras clave
Requiere API key (incluida una demo)



APIs Internas

GET /api/fact - Obtiene nuevo fact con GIF
GET /api/gif?query={palabras}&offset={numero} - Obtiene GIF específico
GET /api/history - Obtiene historial de búsquedas

📊 Base de Datos
Tabla: SearchHistory
sqlCREATE TABLE SearchHistories (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    SearchDate DATETIME NOT NULL,
    FactText TEXT NOT NULL,
    QueryWords VARCHAR(500) NOT NULL,
    GifUrl VARCHAR(500) NOT NULL
);