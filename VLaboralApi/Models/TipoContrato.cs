using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class TipoContrato
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //fpaz: relacion 1 a M con puestos (muchos)
        public virtual ICollection<Puesto> Puestos { get; set; }

    }
}