using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class Puesto
    {
        public Puesto()
        {
            this.Subrubros = new HashSet<SubRubro>(); //fpaz: para la relacion M a M coin subrubros
        }
        
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public ICollection<string> Habilidades { get; set; }
        public string Remuneracion { get; set; }
        public int Vacantes { get; set; } //fpaz: numero de vacantes para el puesto

        //fpaz: relacion M a M con Subrubros
        public virtual ICollection<SubRubro> Subrubros { get; set; }

        //fpaz: relacion 1 a M con Tipo Contrato (uno)
        public int TipoContratoId { get; set; }
        public virtual TipoContrato TipoContrato { get; set; }

        //fpaz: relacion 1 a M con Tipo Disponibilidad (uno)
        public int TipoDisponibilidadId { get; set; }
        public virtual TipoDisponibilidad Disponibilidad { get; set; }

        //fpaz: relacion 1 a M con requisitos (muchos)
        public virtual ICollection<Requisito> Requisitos { get; set; }
    }
}