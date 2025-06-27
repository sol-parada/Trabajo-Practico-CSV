using Npgsql;
using TrabajoPracticoCSV.Domain;
using System.Text;
using System.Threading;

namespace TrabajoPracticoCSV.Infrastructure
{
    public class AlumnoRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public AlumnoRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // Elimina todos los registros de la tabla alumnos de forma eficiente
        public async Task TruncateAsync()
        {
            using var connection = _connectionFactory.CreateConnection() as NpgsqlConnection;
            await connection.OpenAsync();
            using var cmd = new NpgsqlCommand("TRUNCATE TABLE alumnos;", connection);
            await cmd.ExecuteNonQueryAsync();
        }

        // Inserción concurrente, SQL embebido, lotes grandes
        public virtual async Task BulkInsertAsync(IEnumerable<Alumno> alumnos, int batchSize = 5000, int maxDegreeOfParallelism = 6)
        {
            var alumnosList = alumnos.ToList();
            var batches = alumnosList
                .Select((alumno, idx) => new { alumno, idx })
                .GroupBy(x => x.idx / batchSize)
                .Select(g => g.Select(x => x.alumno).ToList())
                .ToList();

            using var throttler = new SemaphoreSlim(maxDegreeOfParallelism);

            var tasks = batches.Select(async batch =>
            {
                await throttler.WaitAsync();
                try
                {
                    using var connection = _connectionFactory.CreateConnection() as NpgsqlConnection;
                    await connection.OpenAsync();
                    using var transaction = await connection.BeginTransactionAsync();

                    var sb = new StringBuilder();
                    sb.Append("INSERT INTO alumnos (apellido, nombre, nro_documento, tipo_documento, fecha_nacimiento, sexo, nro_legajo, fecha_ingreso) VALUES ");

                    for (int j = 0; j < batch.Count; j++)
                    {
                        if (j > 0) sb.Append(",");
                        var a = EscapeSql(batch[j].Apellido);
                        var b = EscapeSql(batch[j].Nombre);
                        var c = EscapeSql(batch[j].NroDocumento);
                        var d = EscapeSql(batch[j].TipoDocumento);
                        var e = batch[j].FechaNacimiento.ToString("yyyy-MM-dd");
                        var f = EscapeSql(batch[j].Sexo);
                        var g = batch[j].NroLegajo;
                        var h = batch[j].FechaIngreso.ToString("yyyy-MM-dd");
                        sb.Append($"('{a}','{b}','{c}','{d}','{e}','{f}',{g},'{h}')");
                    }
                    sb.Append(";");

                    using var cmd = new NpgsqlCommand(sb.ToString(), connection, transaction);
                    cmd.CommandTimeout = 300;
                    await cmd.ExecuteNonQueryAsync();

                    await transaction.CommitAsync();
                }
                finally
                {
                    throttler.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        // Elimina los índices antes de importar
        public async Task DropIndexesAsync()
        {
            using var connection = _connectionFactory.CreateConnection() as NpgsqlConnection;
            await connection.OpenAsync();
            using var cmd = new NpgsqlCommand(@"
                DROP INDEX IF EXISTS idx_alumnos_nro_documento;
                DROP INDEX IF EXISTS idx_alumnos_nro_legajo;
                -- Agrega aquí más índices si tienes
            ", connection);
            await cmd.ExecuteNonQueryAsync();
        }

        // Crea los índices después de importar
        public async Task CreateIndexesAsync()
        {
            using var connection = _connectionFactory.CreateConnection() as NpgsqlConnection;
            await connection.OpenAsync();
            using var cmd = new NpgsqlCommand(@"
                CREATE INDEX IF NOT EXISTS idx_alumnos_nro_documento ON alumnos(nro_documento);
                CREATE INDEX IF NOT EXISTS idx_alumnos_nro_legajo ON alumnos(nro_legajo);
                -- Agrega aquí más índices si tienes
            ", connection);
            await cmd.ExecuteNonQueryAsync();
        }

        // Escapa comillas simples para evitar SQL injection
        private static string EscapeSql(string? value)
        {
            return value?.Replace("'", "''") ?? "";
        }
    }
}