using System.Collections.Generic;

namespace VLaboralApi.Models
{
    public class Rubro
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //fpaz: relacion 1 a M con Subrubros (muchos)
        public virtual ICollection<SubRubro> Subrubros { get; set; }
    }
}