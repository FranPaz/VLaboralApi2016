using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class Postulacion //fpaz: contiene info de un postulante de un puesto en una etapa determinada
    {
        public int Id { get; set; }
        public string Comentario { get; set; }
        public string Valoracion { get; set; }
        public bool PasaEtapa { get; set; }
        public DateTime? Fecha { get; set; }

        //fpaz: relacion 1 a m con PuestoEtapaOferta (uno)
        public int PuestoEtapaOfertaId { get; set; }
        public virtual PuestoEtapaOferta PuestoEtapaOferta { get; set; }

        //fpaz: relacion 1 a m con profesional (uno)
        public int ProfesionalId { get; set; }
        public virtual Profesional Profesional { get; set; }


    }
}