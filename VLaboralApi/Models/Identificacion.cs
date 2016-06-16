using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class IdentificacionProfesional
    {
        public int Id { get; set; }
        public string Valor { get; set; }

        //fpaz: relacion 1 a M con Tipo de identificacion (uno)
        public int TipoIdentificacionProfesionalId { get; set; }
        public virtual TipoIdentificacionProfesional TipoIdentificacionProfesional { get; set; }

        //fpaz: relacion 1 a M con Profesional (uno)
        public int ProfesionalId { get; set; }
        public virtual Profesional Profesional { get; set; }
    }
   
}