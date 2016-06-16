using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class Puesto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public ICollection<string> Habilidades { get; set; }
        public string Remuneracion { get; set; }
        public SubRubro Subrubro { get; set; }
    }
}