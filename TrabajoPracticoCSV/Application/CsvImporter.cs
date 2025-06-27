using CsvHelper;
using System.Globalization;
using TrabajoPracticoCSV.Domain;
using TrabajoPracticoCSV.Infrastructure;

namespace TrabajoPracticoCSV.Application
{
    public class CsvImporter
    {
        private readonly AlumnoRepository _repository;
        private readonly int _batchSize;
        private readonly int _maxDegreeOfParallelism;

        public CsvImporter(AlumnoRepository repository, int batchSize, int maxDegreeOfParallelism)
        {
            _repository = repository;
            _batchSize = batchSize;
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        public async Task<int> ImportAsync(string csvPath)
        {
            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var alumnos = new List<Alumno>();
            int total = 0;

            await foreach (var alumno in csv.GetRecordsAsync<Alumno>())
            {
                alumnos.Add(alumno);
                if (alumnos.Count == _batchSize * _maxDegreeOfParallelism)
                {
                    await _repository.BulkInsertAsync(alumnos, _batchSize, _maxDegreeOfParallelism);
                    total += alumnos.Count;
                    alumnos.Clear();
                }
            }

            if (alumnos.Count > 0)
            {
                await _repository.BulkInsertAsync(alumnos, _batchSize, _maxDegreeOfParallelism);
                total += alumnos.Count;
            }

            return total;
        }
    }
}