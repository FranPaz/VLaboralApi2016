using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class EtapaOferta
    {
        public int Id { get; set; }
        public int IdEtapaAnterior { get; set; }
        public int IdEstapaSiguiente { get; set; }
        public bool EtapaActiva { get; set; } //sirve para saber si la etapa todacia esta activa (true/false)
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaFinTentativa { get; set; } //fecha de fin de la etapa estimada por la empresa

        //fpaz: relacion 1 a m con oferta (uno)
        public int? OfertaId { get; set; }
        public virtual Oferta Oferta { get; set; }

        //fpaz: relacion 1 a m con tipo etapa (uno)
        public int TipoEtapaId { get; set; }
        public virtual TipoEtapa TipoEtapa { get; set; }

        //fpaz: relacion 1 a m con PuestoEtapaOferta (muchos)
        public virtual ICollection<PuestoEtapaOferta> PuestosEtapaOferta { get; set; }
    }

    public class TipoEtapa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //fpaz: relacion 1 a m con etapa oferta (muchos)
        public virtual ICollection<EtapaOferta> EtapasOferta { get; set; }
    }

    public class PuestoEtapaOferta
    {
        public int Id { get; set; }

        //fpaz: relacion 1 a m con EtapaOferta (uno)
        public int EtapaOfertaId { get; set; }
        public virtual EtapaOferta EtapaOferta { get; set; }

        //fpaz: relacion 1 a m con Puestos (uno)
        public int? PuestoId { get; set; }
        public virtual Puesto Puesto { get; set; }

        //fpaz: relacion 1 a m con Postulacion (muchos)
        public virtual ICollection<Postulacion> Postulaciones { get; set; }
    }
}