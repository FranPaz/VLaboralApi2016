using System;

namespace VLaboralApi.ViewModels.Empleados
{
    public abstract class ExperienciaLaboralEmpleadoVM
    {
        public string Puesto { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public DateTime? PeriodoDesde { get; set; }
        public DateTime? PeriodoHasta { get; set; }
    }
}