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

        public DomicilioEmpleadoVM Domicilio { get; set; }

        public Sexo Sexo { get; set; }

        public string Legajo { get; set; }
        public DateTime? FechaInicioVigencia { get; set; }
        public DateTime? FechaFinVigencia { get; set; }

        public int? ProfesionalId { get; set; }

        public ICollection<IdentificacionEmpleadoVM> IdentificacionesEmpleado { get; set; }
        public ICollection<ExperienciaLaboralEmpleadoVM> ExperienciasLaborales { get; set; }
    }

    public abstract class IdentificacionEmpleadoVM
    {
        public string Valor { get; set; }
        public int TipoIdentificacionEmpleadoId { get; set; }
    }

    public abstract class DomicilioEmpleadoVM
    {
        public string PlaceId { get; set; }
        // public Location Location { get; set; }

        public string Calle { get; set; }
        public string Nro { get; set; }
        public string Piso { get; set; }
        public string Dpto { get; set; }

        public string CodigoPostal { get; set; }

        public int? CiudadId { get; set; }
    }
}