using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class Profesional
    {
        public Profesional()
        {
            this.Subrubros = new HashSet<SubRubro>(); //fpaz: para la relacion M a M coin subrubros
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }        
        public string Nacionalidad { get; set; }        
        public DateTime? FechaNac { get; set; }
        public string Domicilio { get; set; }
        public string ObjetivoProfesional { get; set; }
        public string DescripcionCurricular { get; set; }

        //fpaz: relacion 1 a M con Identificacion (muchos). Tiene el array con todos los tipos de identificaciones del profesional y sus valores (Dni, Cuil, Pasaporte)        
        public virtual ICollection<IdentificacionProfesional> IdentificacionesProfesional { get; set; }        

        //fpaz: relacion M a M con Subrubros
        public virtual ICollection<SubRubro> Subrubros { get; set; }

        public override string ToString()
        {
            return Nombre + " " + Apellido;
        }
    }
}