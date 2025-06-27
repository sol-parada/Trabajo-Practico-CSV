using Microsoft.Extensions.Configuration;
using TrabajoPracticoCSV.Infrastructure;
using TrabajoPracticoCSV.Application;
using System.Diagnostics;
using System.IO;
using DotNetEnv;

DotNetEnv.Env.Load("Infra/.env"); // Ajusta la ruta si es necesario

// Carga configuración
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

string port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? throw new Exception("Falta POSTGRES_PORT");
string db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? throw new Exception("Falta POSTGRES_DB");
string user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? throw new Exception("Falta POSTGRES_USER");
string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? throw new Exception("Falta POSTGRES_PASSWORD");

#nullable disable
string connectionString = config.GetConnectionString("DefaultConnection")
    .Replace("${POSTGRES_PORT}", port)
    .Replace("${POSTGRES_DB}", db)
    .Replace("${POSTGRES_USER}", user)
    .Replace("${POSTGRES_PASSWORD}", password);
#nullable restore

// Lee parámetros de configuración
int batchSize = int.TryParse(config["BatchSize"], out var bs) ? bs : 5000;
int maxDegreeOfParallelism = int.TryParse(config["MaxDegreeOfParallelism"], out var mp) ? mp : 8;

// DI manual
var dbFactory = new DbConnectionFactory(connectionString);
var repo = new AlumnoRepository(dbFactory);
var importer = new CsvImporter(repo, batchSize, maxDegreeOfParallelism);

// Lee la ruta del archivo CSV desde la configuración
string? csvPath = config["CsvFilePath"];

if (string.IsNullOrWhiteSpace(csvPath))
{
    Console.WriteLine("No se ha configurado la ruta del archivo CSV en appsettings.json.");
    return;
}

if (!File.Exists(csvPath))
{
    Console.WriteLine($"El archivo CSV '{csvPath}' no existe.");
    return;
}

// Elimina todos los registros de la tabla antes de importar
await repo.TruncateAsync();

// Elimina los índices antes de importar
await repo.DropIndexesAsync();

var stopwatch = Stopwatch.StartNew();
int cantidad = await importer.ImportAsync(csvPath);
stopwatch.Stop();

Console.WriteLine($"Importación completada en {stopwatch.Elapsed.TotalSeconds:F2} segundos.");
Console.WriteLine($"Registros importados: {cantidad}");

// Recrea los índices después de importar
await repo.CreateIndexesAsync();
Console.WriteLine("Índices recreados.");