# FrikiMarvel API

API REST desarrollada con .NET 9 siguiendo los principios de Domain-Driven Design (DDD), SOLID, Clean Architecture y Clean Code.

## 🚀 Características

- **Arquitectura Limpia**: Implementación de DDD con separación clara de capas
- **Base de Datos**: MySQL con Entity Framework Core
- **Health Checks**: Endpoints para monitorear el estado de la API y la base de datos
- **Principios SOLID**: Código mantenible y extensible
- **Inyección de Dependencias**: Configuración completa de DI

## 📋 Prerrequisitos

- .NET 9 SDK
- XAMPP (MySQL)
- Visual Studio 2022 o VS Code

## 🛠️ Configuración

### 1. Configurar XAMPP

1. Instalar XAMPP
2. Iniciar Apache y MySQL desde el panel de control
3. Verificar que MySQL esté corriendo en el puerto 3306

### 2. Configurar la Base de Datos

La API creará automáticamente la base de datos `FrikiMarvelDB` al ejecutarse por primera vez.

### 3. Configurar Marvel API

Para usar los endpoints de Marvel, necesitas obtener las claves de la [Marvel Developer Portal](https://developer.marvel.com/):

1. Ve a [https://developer.marvel.com/](https://developer.marvel.com/)
2. Regístrate o inicia sesión
3. Obtén tu **Public Key** y **Private Key**
4. Configura las claves en `appsettings.json`:

```json
{
  "MarvelApi": {
    "BaseUrl": "https://gateway.marvel.com/v1/public",
    "PublicKey": "tu_public_key_aqui",
    "PrivateKey": "tu_private_key_aqui",
    "TimeoutSeconds": 30
  }
}
```

**⚠️ Importante**: Nunca subas tus claves privadas a repositorios públicos. Usa variables de entorno en producción.

### 4. Configurar archivo de configuración

**⚠️ IMPORTANTE**: Antes de ejecutar el proyecto, debes configurar el archivo `appsettings.json`:

1. Copia el archivo `appsettings.json.example` y renómbralo a `appsettings.json`
2. Edita el archivo `appsettings.json` con tus configuraciones específicas:
   - Configura tu cadena de conexión a la base de datos
   - Agrega tus claves de Marvel API (PublicKey y PrivateKey)
   - Ajusta cualquier otra configuración necesaria

```bash
# Copiar y renombrar el archivo de ejemplo
cp appsettings.json.example appsettings.json
```

### 5. Ejecutar la Aplicación

```bash
cd FrikiMarvelApi
dotnet restore
dotnet run
```

La API estará disponible en:
- HTTP: `http://localhost:5150`

## 📚 Endpoints Disponibles

También puedes acceder a Swagger (en caso de haber levantado el api en el puerto por defecto - 5150):
- Swagger: `http://localhost:5150/swagger`

### Health Check

#### GET `/api/health`
Verifica el estado de la API y la conexión a la base de datos.

**Respuesta exitosa (200):**
```json
{
  "status": "Healthy",
  "isHealthy": true,
  "database": "Connected",
  "checkedAt": "2024-01-15T10:30:00Z",
  "details": {
    "Database": "Connected and accessible",
    "Environment": "Development",
    "MachineName": "DESKTOP-ABC123",
    "OSVersion": "Microsoft Windows NT 10.0.26100.0"
  },
  "message": "API funcionando correctamente"
}
```

#### GET `/api/health/ping`
Endpoint simple para verificar que la API está funcionando.

**Respuesta:**
```json
{
  "message": "Pong! API funcionando",
  "timestamp": "2024-01-15T10:30:00Z",
  "version": "1.0.0"
}
```

## 🏗️ Arquitectura

### Estructura del Proyecto

```
FrikiMarvelApi/
├── Domain/                    # Capa de Dominio (DDD)
│   ├── Entities/             # Entidades del dominio
│   └── Interfaces/           # Contratos/Interfaces
├── Application/              # Capa de Aplicación
│   └── Services/            # Servicios de aplicación
├── Infrastructure/           # Capa de Infraestructura
│   └── Persistence/         # Persistencia de datos
│       ├── Repositories/    # Implementación de repositorios
│       └── AppDbContext.cs  # Contexto de Entity Framework
├── Api/                     # Capa de Presentación (API)
│   └── Controllers/         # Controladores de la API
└── Program.cs              # Configuración de la aplicación
```

### Principios Implementados

#### Domain-Driven Design (DDD)
- **Entidades**: `Character`, `BaseEntity`
- **Repositorios**: Interfaces en Domain, implementación en Infrastructure
- **Servicios de Dominio**: Lógica de negocio encapsulada

#### SOLID
- **S** - Single Responsibility: Cada clase tiene una responsabilidad
- **O** - Open/Closed: Extensible sin modificar código existente
- **L** - Liskov Substitution: Interfaces bien definidas
- **I** - Interface Segregation: Interfaces específicas
- **D** - Dependency Inversion: Dependencias inyectadas

#### Clean Architecture
- **Independencia de frameworks**: Entity Framework aislado en Infrastructure
- **Testabilidad**: Fácil testing con inyección de dependencias
- **Independencia de base de datos**: Cambio de BD sin afectar dominio

## 🔧 Configuración de Desarrollo

### Connection Strings

**Producción** (`appsettings.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=FrikiMarvelDB;Uid=root;Pwd=;"
  }
}
```

**Desarrollo** (`appsettings.Development.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=FrikiMarvelDB_Dev;Uid=root;Pwd=;"
  }
}
```

### Logging

En desarrollo se incluye logging de consultas SQL para debugging:
```json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

## 🧪 Testing

Para probar los endpoints, puedes usar:

1. **Swagger UI**: Disponible en `/swagger` en desarrollo
2. **Postman**: Importar la colección desde `FrikiMarvelApi_Postman_Collection.json`
3. **HTTP Files**: Usar el archivo `FrikiMarvelApi.http` con REST Client
4. **curl**: Ejemplos de comandos curl

### Importar Colección de Postman

1. Abre Postman
2. Haz clic en "Import"
3. Selecciona el archivo `FrikiMarvelApi_Postman_Collection.json`
4. La colección se importará con todos los endpoints preconfigurados
5. Configura la variable `base_url` en el entorno de Postman si es necesario

### Ejemplo de Testing con curl

```bash
# Health check
curl -X GET "https://localhost:5001/api/health"

# Registrar usuario
curl -X POST "http://localhost:5150/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Juan Pérez",
    "identification": "12345678",
    "email": "juan@example.com",
    "password": "password123"
  }'
```

## 📝 Notas Importantes

- **⚠️ CRÍTICO**: Siempre copia `appsettings.json.example` a `appsettings.json` antes de ejecutar el proyecto
- La base de datos se crea automáticamente al ejecutar la aplicación
- Se implementa soft delete para mantener integridad de datos
- Los timestamps se manejan automáticamente
- La API incluye manejo de errores y respuestas consistentes
- Todos los endpoints devuelven respuestas en formato JSON estructurado
- **Autenticación JWT**: Todas las rutas están protegidas excepto login, registro y health check