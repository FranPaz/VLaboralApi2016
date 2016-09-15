using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public abstract class Estudio
    {
        public int Id { get; set; }
        public string LugarEstudio { get; set; }
        public string Titulacion { get; set; }
        public string Descripcion { get; set; }
        public double Nota { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

           
    }

    [Table("Educacion")]
    public class Educacion:Estudio
    {
        //fpaz: relacion 1 a m con tiponivel de estudio (uno)
        public int TipoNivelEstudioId { get; set; }
        public virtual TipoNivelEstudio TipoNivelEstudio { get; set; }

        //fpaz: 1 a m con profesional (uno)
        public int ProfesionalId { get; set; }
        public virtual Profesional Profesional { get; set; }
    }

    [Table("Cursos_Certificaciones")]
    public class Curso_Certificacion : Estudio
    {
        public string EntidadCertificante { get; set; }

        //fpaz: 1 a m con profesional (uno)
        public int ProfesionalId { get; set; }
        public virtual Profesional Profesional { get; set; }
    }
}