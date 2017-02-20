using System.Collections.Generic;

namespace VLaboralApi.Models
{
    public class Requisito
    {
        public int Id { get; set; }
        public string Observacion { get; set; }
        public bool Excluyente { get; set; }
        public bool AutoVerificar { get; set; }

        //kikexp: relacion 1 a M con TipoRequisito
        public int TipoRequisitoId { get; set; }
        public virtual TipoRequisito TipoRequisito { get; set; }

        //fpaz: relacion 1 a M con puesto (uno)
        public int PuestoId { get; set; }
        public virtual Puesto Puesto { get; set; }

        public virtual ICollection<ValoresRequisito> ValoresRequisito { get; set; }
    }

    public class ValoresRequisito
    {
        public int Id { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public int? Desde { get; set; }
        public int? Hasta { get; set; }
        
        //sluna: relacion 1 a M con Requisito
        public int RequisitoId { get; set; }
        public virtual Requisito Requisito { get; set; }
    }
}