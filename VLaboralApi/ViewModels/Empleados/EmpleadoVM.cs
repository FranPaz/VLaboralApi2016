using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLaboralApi.Models;
using VLaboralApi.Models.Ubicacion;

namespace VLaboralApi.ViewModels.Empleados
{
    public class EmpleadoVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Nacionalidad { get; set; }
        public DateTime? FechaNac { get; set; }

        public int? DomicilioId { get; set; }
        public virtual Domicilio Domicilio { get; set; }

        public Sexo Sexo { get; set; }

        public string Legajo { get; set; }
        public DateTime? FechaInicioVigencia { get; set; }
        public DateTime? FechaFinVigencia { get; set; }

        //fpaz: 1 a muchos con empresa (uno)
        public int? EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }

        //fpaz: 1 a m con profesional (uno)
        public int? ProfesionalId { get; set; }
        public virtual Profesional Profesional { get; set; }

        //fpaz: relacion 1 a M con Identificacion (muchos). Tiene el array con todos los tipos de identificaciones del Empleado y sus valores (Dni, Cuil, Pasaporte)        
        public virtual ICollection<IdentificacionEmpleado> IdentificacionesEmpleado { get; set; }

        ////fpaz: relacion M a M con Subrubros
        //public virtual ICollection<SubRubro> Subrubros { get; set; }

        ////fpaz: relacion 1 a M con ExperienciaLaboral (muchos)
        public virtual ICollection<ExperienciaLaboralEmpleado> ExperienciasLaborales { get; set; }
    }
}