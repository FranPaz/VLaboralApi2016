using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class IdentificacionProfesional
    {
        public int Id { get; set; }
        public string Valor { get; set; }

        //fpaz: relacion 1 a M con Tipo de identificacion Profesional (uno)
        public int TipoIdentificacionProfesionalId { get; set; }
        public virtual TipoIdentificacionProfesional TipoIdentificacionProfesional { get; set; }

        //fpaz: relacion 1 a M con Profesional (uno)
        public int ProfesionalId { get; set; }
        public virtual Profesional Profesional { get; set; }
    }

    public class IdentificacionEmpleado
    {
        public int Id { get; set; }
        public string Valor { get; set; }

        //fpaz: relacion 1 a M con Tipo de identificacion Profesional (uno)
        public int TipoIdentificacionEmpleadoId { get; set; }
        public virtual TipoIdentificacionEmpleado TipoIdentificacionEmpleado { get; set; }

        //fpaz: relacion 1 a M con Profesional (uno)
        public int EmpleadoId { get; set; }
        public virtual Empleado Empleado { get; set; }
    }

    public class IdentificacionEmpresa
    {
        public int Id { get; set; }
        public string Valor { get; set; }

        //fpaz: relacion 1 a M con Tipo de identificacion Empresa (uno)
        public int TipoIdentificacionEmpresaId { get; set; }
        public virtual TipoIdentificacionEmpresa TipoIdentificacionEmpresa { get; set; }

        //fpaz: relacion 1 a M con Empresa (uno)
        public int EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
   
}