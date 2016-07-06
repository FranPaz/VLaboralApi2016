using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class Oferta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicioConvocatoria { get; set; }
        public DateTime FechaFinConvocatoria { get; set; }
        public bool Publica { get; set; }

        //iafar:Relacion 1 a M con puestos (Muchos)
        public virtual ICollection<Puesto> Puestos { get; set; }

        //iafar: relacion 1 a m con empresa (1)
        public int EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }

        //iafar: relacion 1 a m con OfertaEstado (m)
        public virtual ICollection<OfertaEstado> OfertaEstados { get; set; }



    }
}