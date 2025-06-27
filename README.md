# Trabajo Práctico: Importador de CSV en C#

Este proyecto es una aplicación de consola en C# que importa datos desde un archivo CSV a una base de datos PostgreSQL. Está pensado para ejecutarse directamente en tu sistema operativo, de forma rápida y portable.

---

## Requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL instalado y corriendo (local o remoto)

---

## Estructura del proyecto
TrabajoPracticoCSV/ ├── Application/ ├── Data/ │   └── alumnos.csv ├── Domain/ ├── Infrastructure/ ├── Program.cs ├── TrabajoPracticoCSV.csproj ├── appsettings.json ├── Infra/ │   └── .env └── Properties/


---

## Configuración de la base de datos

1. Crea una base de datos PostgreSQL y una tabla `alumnos` con la estructura adecuada.
2. Edita el archivo `Infra/.env` con tus datos de conexión:

POSTGRES_USER=tu_usuario POSTGRES_PASSWORD=tu_contraseña POSTGRES_DB=tu_base POSTGRES_PORT=5432


---

## Configuración de la aplicación

- El archivo `appsettings.json` define la cadena de conexión y la ruta al CSV.
- Por defecto, la ruta del CSV es `Data/alumnos.csv`.
- Asegúrate de que el archivo `Data/alumnos.csv` exista y tenga el formato correcto.

---

## Ejecución

1. Abre una terminal en la carpeta `TrabajoPracticoCSV/TrabajoPracticoCSV`.
2. Restaura dependencias y ejecuta:

    ```sh
    dotnet restore
    dotnet run
    ```

La aplicación conectará a la base, importará el CSV y mostrará cuántos registros se importaron y el tiempo de ejecución.

---

## Personalización

- Para importar otro archivo, cambia la ruta en `appsettings.json` (`CsvFilePath`).
- Puedes ajustar la configuración de la base de datos en el archivo `.env`.

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

---

## Autor

Sol Parada
