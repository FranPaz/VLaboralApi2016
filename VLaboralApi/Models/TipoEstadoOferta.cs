using System.Collections.Generic;

namespace VLaboralApi.Models
{
    public class TipoEstadoOferta
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

      //iafar: relacion 1 a m con OfertaEstado (m)
        public virtual ICollection<OfertaEstado> OfertaEstados { get; set; }




    }
}