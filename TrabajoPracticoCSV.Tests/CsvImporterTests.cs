using System.IO;
using System.Threading.Tasks;
using TrabajoPracticoCSV.Application;
using TrabajoPracticoCSV.Domain;
using Xunit;
using System.Collections.Generic;

namespace TrabajoPracticoCSV.Tests
{
    public class CsvImporterTests
    {
        // Fake repository para capturar los alumnos insertados
        private class FakeAlumnoRepository : Infrastructure.AlumnoRepository
        {
            public List<Alumno> Insertados = new();
            public FakeAlumnoRepository() : base(null!) { }
            public override async Task BulkInsertAsync(IEnumerable<Alumno> alumnos, int batchSize = 5000, int maxDegreeOfParallelism = 6)
            {
                Insertados.AddRange(alumnos);
                await Task.CompletedTask;
            }
        }

        [Fact]
        public async Task ImportAsync_ImportaTodosLosAlumnos()
        {
            // Arrange: crea un archivo CSV temporal
            var csv = "apellido,nombre,nro_documento,tipo_documento,fecha_nacimiento,sexo,nro_legajo,fecha_ingreso\n" +
                      "Perez,Juan,12345678,DNI,2000-01-01,M,1001,2020-03-01\n" +
                      "Gomez,Ana,87654321,DNI,1999-05-10,F,1002,2021-04-15\n";
            var tempFile = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFile, csv);

            var repo = new FakeAlumnoRepository();
            var importer = new CsvImporter(repo, batchSize: 1, maxDegreeOfParallelism: 1);

            // Act
            int cantidad = await importer.ImportAsync(tempFile);

            // Assert
            Assert.Equal(2, cantidad);
            Assert.Equal(2, repo.Insertados.Count);
            Assert.Equal("Perez", repo.Insertados[0].Apellido);
            Assert.Equal("Gomez", repo.Insertados[1].Apellido);

            File.Delete(tempFile);
        }
    }
}