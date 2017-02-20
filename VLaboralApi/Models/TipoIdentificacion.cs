using System.Collections.Generic;

namespace VLaboralApi.Models
{
    public class TipoIdentificacionProfesional //fpaz: tiene todos los tipos de identificacion que puede tener un profesional
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //fpaz: relacion 1 a M con identificacionesProfesional
        public virtual ICollection<IdentificacionProfesional> IdentificacionesProfesional { get; set; }
    }

    public class TipoIdentificacionEmpleado //fpaz: tiene todos los tipos de identificacion que puede tener un Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //fpaz: relacion 1 a M con identificacionesProfesional
        public virtual ICollection<IdentificacionEmpleado> IdentificacionEmpleados { get; set; }

    }
    public class TipoIdentificacionEmpresa //fpaz: tiene todos los tipos de identificacion que puede tener una Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //fpaz: relacion 1 a M con identificacionesEmpresa
        //public virtual ICollection<IdentificacionProfesional> IdentificacionesProfesional { get; set; }
    }
}