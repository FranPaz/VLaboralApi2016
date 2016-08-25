using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class TipoRequisito
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public bool Verificable { get; set; }
        public bool Multiple { get; set; }

        public bool Habilitado { get; set; }

        //kikexp: uno a muchos con requisito (muchos)
        public virtual ICollection<Requisito> Requisitos { get; set; }

        //sluna: uno a muchos con ValoresTipoRequisito (muchos)
        public virtual ICollection<ValoresTipoRequisito> ValoresTipoRequisito { get; set; }
    }
     
    public class ValoresTipoRequisito
    {
        public int Id { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }

        public int? Desde { get; set; }
        public int? Hasta { get; set; }
        
        //sluna: relacion 1 a M con TipoRequisito (uno)
        public int TipoRequisitoId { get; set; }
        public virtual TipoRequisito TipoRequisito { get; set; }

    }
 
}