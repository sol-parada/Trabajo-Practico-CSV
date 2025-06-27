# Trabajo Práctico: Importador de CSV en C#

Este proyecto es una aplicación de consola en C# que importa datos desde un archivo CSV a una base de datos PostgreSQL. Está diseñado para ejecutarse directamente desde el sistema operativo de forma rápida y portable.

## Historial de cambios
- 27/06/2025: Se realizaron cambios importantes en la estructura del proyecto y actualización de mensajes de commit.

## Requisitos

- .NET 8.0 SDK
- PostgreSQL instalado y en ejecución (local o remoto)

## Configuración de la base de datos

1. Crear una base de datos PostgreSQL y una tabla `alumnos` con la estructura adecuada (puede ser local o remota).

## Estructura de la tabla `alumnos`
La tabla `alumnos` debe tener la siguiente estructura en la base de datos PostgreSQL:

- **apellido:** TEXT  
- **nombre:** TEXT  
- **nro_documento:** TEXT  
- **tipo_documento:** TEXT  
- **fecha_nacimiento:** DATE  
- **sexo:** TEXT  
- **nro_legajo:** INT  
- **fecha_ingreso:** DATE  

2. Editar el archivo `Infra/.env` con los datos de conexión:

    ```
    POSTGRES_PORT=5432
    POSTGRES_DB=su_base_de_datos
    POSTGRES_USER=su_usuario
    POSTGRES_PASSWORD=su_contraseña
    ```

    **Nota:** El puerto por defecto de PostgreSQL es 5432. Si utiliza otro puerto, debe modificarlo en el archivo.

## Configuración de la aplicación

- El archivo `appsettings.json` define la cadena de conexión y la ruta al archivo CSV.
- Por defecto, la ruta del CSV es `Data/alumnos.csv`.
- Asegúrese de que el archivo `Data/alumnos.csv` exista y tenga el formato correcto.

## Ejecución

1. Abrir una terminal en la carpeta `TrabajoPracticoCSV/TrabajoPracticoCSV`.
2. Compilar la aplicación:

    ```bash
    dotnet publish -c Release -o out
    ```

3. Ejecutar:

    - **En Windows:**
      ```cmd
      .\out\TrabajoPracticoCSV.exe
      ```
    - **En Linux/macOS:**
      ```bash
      chmod +x ./out/TrabajoPracticoCSV
      ./out/TrabajoPracticoCSV
      ```

La aplicación se conectará a la base de datos, importará el archivo CSV y mostrará la cantidad de registros importados junto con el tiempo de ejecución.

## Personalización

### Cambiar archivo de origen

Para importar otro archivo, modifique la ruta en `appsettings.json` (`CsvFilePath`).

### Configuración de base de datos

Ajuste la configuración de la base de datos en el archivo `Infra/.env`.

### Optimización de rendimiento

La aplicación permite ajustar dos parámetros clave para optimizar la importación según la capacidad del sistema:

- **BatchSize:** cantidad de registros procesados por lote
- **MaxDegreeOfParallelism:** cantidad de procesos concurrentes

En `appsettings.json` puede modificar estos valores:

```json
{
  "BatchSize": 10000,
  "MaxDegreeOfParallelism": 16
}
```

## Dependencias principales

- **CsvHelper:** Procesamiento de archivos CSV
- **Dapper:** Micro ORM para .NET
- **Npgsql:** Proveedor de datos PostgreSQL para .NET
- **DotNetEnv:** Carga de variables de entorno desde archivos .env
- **Microsoft.Extensions.Configuration:** Sistema de configuración de .NET

## Solución de problemas

Si encuentra un error de conexión (ejemplo: `"Failed to connect to 127.0.0.1:5432"`), verifique que:

- PostgreSQL esté ejecutándose
- Los datos de conexión en el archivo `.env` sean correctos
- El puerto especificado esté disponible

## Características técnicas

- Código modularizado y documentado
- Proceso de importación eficiente y asincrónico
- Soporte para procesamiento por lotes
- Ejecución multiplataforma

---

**Autor:**  
Sol Parada
