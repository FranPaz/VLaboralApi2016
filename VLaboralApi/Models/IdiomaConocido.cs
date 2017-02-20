using System.Collections.Generic;

namespace VLaboralApi.Models
{
    public class IdiomaConocido
    {
        public int Id { get; set; }

        //fpaz: relacion 1 a m con Idioma (uno)
        public int IdiomaId { get; set; }
        public virtual Idioma Idioma { get; set; }

        //fpaz: relacion 1 a m conCompetencia idioma (uno)
        public int CompetenciaIdiomaId { get; set; }
        public virtual CompetenciaIdioma CompetenciaIdioma { get; set; }

        //fpaz: 1 a m con profesional (uno)
        public int ProfesionalId { get; set; }
        public virtual Profesional Profesional { get; set; }

    }

    public class Idioma
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //fpaz: relacion 1 a m con IdiomasConocidos (muchos)
        public virtual ICollection<IdiomaConocido> IdiomasConocidos { get; set; }

    }

    public class CompetenciaIdioma
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //fpaz: relacion 1 a m con IdiomasConocidos (muchos)
        public virtual ICollection<IdiomaConocido> IdiomasConocidos { get; set; }
    }
}