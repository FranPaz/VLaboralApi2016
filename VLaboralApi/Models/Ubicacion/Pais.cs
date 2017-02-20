using System.Collections.Generic;

namespace VLaboralApi.Models.Ubicacion
{
    public class Pais
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
    }

    public class Provincia
    {
        public int Id { get; set; }

        public int PaisId { get; set; }
        public virtual Pais Pais { get; set; }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
    }

    public class Ciudad
    {
        public int Id { get; set; }

        public int ProvinciaId { get; set; }
        public virtual Provincia Provincia { get; set; }

        //public int PaisId { get; set; }
        //public virtual Pais Pais { get; set; }

        public string CodigoPostal { get; set; }
        public string Nombre { get; set; }


        public virtual ICollection<Domicilio> Domicilios { get; set; }
    }
}