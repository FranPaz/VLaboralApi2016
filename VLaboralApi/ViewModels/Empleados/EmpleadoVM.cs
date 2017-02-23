using System;
using System.Collections.Generic;
using VLaboralApi.Models;
using VLaboralApi.Models.Ubicacion;

namespace VLaboralApi.ViewModels.Empleados
{
    public class EmpleadoVM
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Nacionalidad { get; set; }
        public DateTime? FechaNac { get; set; }
      
        public virtual DomicilioEmpleadoVM Domicilio { get; set; }

        public Sexo Sexo { get; set; }

        public string Legajo { get; set; }
        public DateTime? FechaInicioVigencia { get; set; }
        public DateTime? FechaFinVigencia { get; set; }

        public int? ProfesionalId { get; set; }
             
        public virtual ICollection<IdentificacionEmpleadoVM> IdentificacionesEmpleado { get; set; }
        public virtual ICollection<ExperienciaLaboralEmpleadoVM> ExperienciasLaborales { get; set; }
    }
}