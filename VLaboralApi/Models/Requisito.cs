using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class Requisito
    {
        public int Id { get; set; }
        public string Valor { get; set; }
        public bool Excluyente { get; set; }

        //fpaz: relacion 1 a M con puesto (uno)
        public int PuestoId { get; set; }
        public virtual Puesto Puesto { get; set; }

        //kikexp: relacion 1 a M con TipoRequisito
        public int TipoRequisitoId { get; set; }
        public virtual TipoRequisito TipoRequisito { get; set; }

    }
}