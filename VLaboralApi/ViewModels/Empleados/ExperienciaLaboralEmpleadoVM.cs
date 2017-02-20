using System;

namespace VLaboralApi.ViewModels.Empleados
{
    public class ExperienciaLaboralEmpleado
    {
        public int Id { get; set; }
        public string Puesto { get; set; }
        public string Descripcion { get; set; }
        public string EmpresaExterna { get; set; }
        public string Ubicacion { get; set; }
        public bool isVerificada { get; set; }
        public DateTime? PeriodoDesde { get; set; }
        public DateTime? PeriodoHasta { get; set; }
        public DateTime? FechaCreacion { get; set; }

        //fpaz: 1 a muchos con empresa (uno)
        public int? EmpresaId { get; set; }
    }
}