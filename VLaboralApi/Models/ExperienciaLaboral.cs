using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class ExperienciaLaboral
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
        public virtual Empresa Empresa { get; set; }

        //fpaz: 1 a m con profesional (uno)
        public int ProfesionalId { get; set; }
        public virtual Profesional Profesional { get; set; }

        public string idUsuarioCreacion { get; set; }

        //iafar: relacion 1 a 1..0 con Verificacion de Experiencia laboral (1..0)
        public virtual VerificacionExperienciaLaboral VerificacionExperienciaLaboral { get; set; }
    }
}