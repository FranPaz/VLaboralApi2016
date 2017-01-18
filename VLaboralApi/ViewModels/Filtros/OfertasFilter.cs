using System;
using System.Collections.Generic;
using VLaboralApi.ViewModels.Filtros;

namespace VLaboralApi.ViewModels.Ofertas
{
    public enum OfertasFilterOptions
    {
        Rubros, DisponibilidadHoraria, TipoContratacion, Ubicaciones
    }

    public enum OfertasOrderByOptions
    {
        FechaInicioConvocatoria, FechaFinConvocatoria
    }

    public class OfertasOptionsBindingModel
    {
        public List<OfertasFilterOptions> Filters { get; set; }
    }

    public class OfertasQueryBindingModel : SearchFilterQueryBindingModel
    {
        public OfertasOrderByOptions OrderBy { get; set; }
        public List<int> Rubros { get; set; }
        public List<int> DisponibilidadHoraria { get; set; }
        public List<int> TipoContratacion { get; set; }
        public List<int> Ubicaciones { get; set; }
    }
    
}