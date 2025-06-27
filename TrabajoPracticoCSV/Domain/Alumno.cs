using CsvHelper.Configuration.Attributes;

namespace TrabajoPracticoCSV.Domain
{
    public class Alumno
    {
        [Name("apellido")]
        public string? Apellido { get; set; }

        [Name("nombre")]
        public string? Nombre { get; set; }

        [Name("nro_documento")]
        public string? NroDocumento { get; set; }

        [Name("tipo_documento")]
        public string? TipoDocumento { get; set; }

        [Name("fecha_nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Name("sexo")]
        public string? Sexo { get; set; }

        [Name("nro_legajo")]
        public int NroLegajo { get; set; }

        [Name("fecha_ingreso")]
        public DateTime FechaIngreso { get; set; }
    }
}