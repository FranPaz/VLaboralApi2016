using System.Collections.Generic;
using VLaboralApi.ViewModels.Filtros;


namespace VLaboralApi.ViewModels.Profesionales
{
   
    public enum ProfesionalesFilterOptions
    {
        Rubros, Valoraciones, Ubicaciones
    }

    public enum ProfesionalesOrderByOptions
    {
        NombreCompleto, Valoracion, NombreCompletoDesc, ValoracionDesc
    }

    public class ProfesionalesOptionsBindingModel
    {
        public List<ProfesionalesFilterOptions> Filters { get; set; }
    }

    public class ProfesionalesQueryBindingModel : SearchFilterQueryBindingModel
    {
        public ProfesionalesOrderByOptions OrderBy { get; set; }
        public List<int> Rubros { get; set; }
        public List<int> Valoraciones { get; set; }
        public List<int> Ubicaciones { get; set; }
    }

}