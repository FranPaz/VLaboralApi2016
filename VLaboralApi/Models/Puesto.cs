using System.Collections.Generic;
using VLaboralApi.Models.Ubicacion;

namespace VLaboralApi.Models
{
    public class Puesto
    {
        //public Puesto()
        //{
        //    this.Subrubros = new HashSet<SubRubro>(); //fpaz: para la relacion M a M coin subrubros
        //}

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public int? DomicilioId { get; set; }
        public virtual Domicilio Domicilio { get; set; }

        public string Habilidades { get; set; }
        public string Remuneracion { get; set; }
        public int Vacantes { get; set; } //fpaz: numero de vacantes para el puesto

        //iafar: Relacion 1 a M con Oferta (1)
        public int OfertaId { get; set; }
        public virtual Oferta Oferta { get; set; }

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

        //fpaz: relacion 1 a M con PuestoEtapaOferta (muchos)
        public virtual ICollection<PuestoEtapaOferta> PuestoEtapasOferta { get; set; }
    }
}