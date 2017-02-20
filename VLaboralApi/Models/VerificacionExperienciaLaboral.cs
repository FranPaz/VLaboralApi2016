using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VLaboralApi.Models
{
    public class VerificacionExperienciaLaboral
    {
       [ForeignKey("ExperienciaLaboral")]
        public int Id { get; set; }
        public string Comentario { get; set; }
        public int Valoracion { get; set; }
        public DateTime FechaVerificacion { get; set; }

        //iafar: relacion 1 a 1..0 con Experiencia Laboral (1)
        public virtual ExperienciaLaboral ExperienciaLaboral { get; set; }

    }
}