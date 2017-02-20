using System.Collections.Generic;

namespace VLaboralApi.Models
{
    public class SubRubro
    {
        public SubRubro()
        {
            this.Puestos = new HashSet<Puesto>(); //fpaz: para la relacion M a M coin subrubros
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //fpaz: relacion 1 a M con Rubros (uno)
        public int RubroId { get; set; }
        public virtual Rubro Rubro { get; set; }

        //fpaz: M a M con puestos
        public virtual ICollection<Puesto> Puestos { get; set; }

        //fpaz: M a M con profesionales
        public virtual ICollection<Profesional> Profesionales { get; set; }
    }
}