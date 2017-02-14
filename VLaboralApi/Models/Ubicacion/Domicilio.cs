using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models.Ubicacion
{
    public class Domicilio
    {

        public int Id { get; set; }

        public string PlaceId { get; set; }
        public Location Location { get; set; }

        public string Calle { get; set; }
        public string Nro { get; set; }
        public string Piso { get; set; }
        public string Dpto { get; set; }

        public string CodigoPostal { get; set; }

        public int? CiudadId { get; set; }
        public virtual Ciudad Ciudad { get; set; }

        public virtual ICollection<Puesto> Puestos { get; set; }

        public virtual ICollection<Profesional> Profesionales { get; set; }
        
        public virtual ICollection<Empleado> Empleados { get; set; }

        public virtual ICollection<Empresa> Empresas { get; set; }

    }

    public class Location
    {
        public Location(double? lat, double? lng)
        {
            Lat = lat;
            Lng = lng;
        }

        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}