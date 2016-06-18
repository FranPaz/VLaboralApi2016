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

        //kikexp: uno a muchos con requisito (muchos)
        ICollection<Requisito> Requisitos { get; set; }
    }
}