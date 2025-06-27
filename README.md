# Trabajo Práctico: Importador de CSV en C#

Este proyecto es una aplicación de consola en C# que importa datos desde un archivo CSV a una base de datos PostgreSQL. Está pensado para ejecutarse directamente desde el sistema operativo, de forma rápida y portable.

---

## Requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL instalado y corriendo (local o remoto)

---

## Configuración de la base de datos

1. Crear una base de datos PostgreSQL y una tabla `alumnos` con la estructura adecuada (puede ser local o remota).
2. Editar el archivo `Infra/.env` con sus datos de conexión:

POSTGRES_USER=su_usuario POSTGRES_PASSWORD=su_contraseña POSTGRES_DB=su_base POSTGRES_PORT=5432
> **Nota:** El puerto por defecto de PostgreSQL es `5432`. Si usa otro, debe cambiarlo acá.

---

## Configuración de la aplicación

- El archivo `appsettings.json` define la cadena de conexión y la ruta al CSV.
- Por defecto, la ruta del CSV es `Data/alumnos.csv`.
- Debe asegurarse de que el archivo `Data/alumnos.csv` exista y tenga el formato correcto.

---

## Ejecución

1. Abrir una terminal en la carpeta `TrabajoPracticoCSV/TrabajoPracticoCSV`.
2. Para compilar la aplicación:
      ```sh
       dotnet publish -c Release -o out
       ```
3. Para ejecutar:
   •	En Windows:
       ```sh
       .\out\TrabajoPracticoCSV.exe
       ```
   •	En Linux/macOS:
       ```sh
        chmod +x ./out/TrabajoPracticoCSV
         ./out/TrabajoPracticoCSV
       ```


La aplicación conectará a la base, importará el CSV y mostrará cuántos registros se importaron y el tiempo de ejecución.

---

## Personalización

- Para importar otro archivo, cambiar la ruta en `appsettings.json` (`CsvFilePath`).
- Puede ajustar la configuración de la base de datos en el archivo `.env`.
  
- **Ajuste de rendimiento:**
  La aplicación permite ajustar dos parámetros clave para optimizar la importación según la capacidad de su sistema:
•	BatchSize: cantidad de registros procesados por lote.
•	MaxDegreeOfParallelism: cantidad de procesos concurrentes.
  En `appsettings.json` puede modificar los valores de `BatchSize` y `MaxDegreeOfParallelism` para optimizar la velocidad de importación según su sistema operativo:
  - Ejemplo:
    ```json
    {
      "BatchSize": 10000,
      "MaxDegreeOfParallelism": 16
    }
    ```
    
---

## Dependencias principales

- CsvHelper
- Dapper
- Npgsql
- DotNetEnv
- Microsoft.Extensions.Configuration

---

## Notas

- El código está modularizado y documentado.
- El proceso de importación es eficiente y asincrónico.
- Si ve un error de conexión (por ejemplo, "Failed to connect to 127.0.0.1:5432"), debe asegurarse de que PostgreSQL esté corriendo y que los datos de `.env` sean correctos.


---

## Autor

Sol Parada
