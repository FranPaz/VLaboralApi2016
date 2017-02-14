using System;
using System.Collections.Generic;
using VLaboralApi.Models;
using VLaboralApi.ViewModels.Filtros;

namespace VLaboralApi.ViewModels.Empleados
{
    public enum EmpleadosFilterOptions
    {
        Sexo, Ubicaciones, Estado
    }

    public enum EmpleadosOrderByOptions
    {
        Nombre, Legajo, FechaInicioRelacionLaboral
    }

    public class EmpleadosOptionsBindingModel
    {
        public List<EmpleadosFilterOptions> Filters { get; set; }
    }

    public class EmpleadosQueryBindingModel : SearchFilterQueryBindingModel
    {
        public EmpleadosOrderByOptions OrderBy { get; set; }
        public List<Sexo> Sexos { get; set; }
        public List<int> Ubicaciones { get; set; }
        public List<EstadoEmpleado> Estados { get; set; }
    }

    public enum EstadoEmpleado
    {
        Activo, Baja
    }
}