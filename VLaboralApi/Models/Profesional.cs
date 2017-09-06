using System;
using System.Collections.Generic;
using VLaboralApi.Models.Ubicacion;

namespace VLaboralApi.Models
{
    public class Profesional
    {
        public Profesional()
        {
            this.Subrubros = new HashSet<SubRubro>(); //fpaz: para la relacion M a M coin subrubros
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }        
        public string Nacionalidad { get; set; }        
        public DateTime? FechaNac { get; set; }
        public string UrlImagenPerfil { get; set; }
        public int? DomicilioId { get; set; }
        public virtual Domicilio Domicilio { get; set; }
        public string UrlCurriculum { get; set; }

        public string ObjetivoProfesional { get; set; }
        public string DescripcionCurricular { get; set; }
        public string Habilidades { get; set; }
        public bool IdentidadVerificada { get; set; }
        public string Sexo { get; set; }
        public double ValoracionPromedio { get; set; }
        //fpaz: relacion 1 a M con Identificacion (muchos). Tiene el array con todos los tipos de identificaciones del profesional y sus valores (Dni, Cuil, Pasaporte)        
        public virtual ICollection<IdentificacionProfesional> IdentificacionesProfesional { get; set; }        

        //fpaz: relacion M a M con Subrubros
        public virtual ICollection<SubRubro> Subrubros { get; set; }

        //fpaz: relacion 1 a M con ExperienciaLaboral (muchos)
        public virtual ICollection<ExperienciaLaboral> ExperienciasLaborales { get; set; }

        //fpaz: relacion 1 a M con Estudios
        public virtual ICollection<Curso_Certificacion> Cursos { get; set; }

        public virtual ICollection<Educacion> Educaciones { get; set; }

        //fpaz: relacion 1 a m con IdiomasConocidos (muchos)
        public virtual ICollection<IdiomaConocido> IdiomasConocidos { get; set; }

        //fpaz: relacion 1 a m con Empleados (muchos)
        public virtual ICollection<Empleado> Empleados { get; set; }

        public override string ToString()
        {
            return Nombre + " " + Apellido;
        }

        //kike: imagenes profesional
        public virtual ICollection<ImagenProfesional> ImagenesProfesional { get; set; }

        //kike: files del profesional
        //public virtual ICollection<FileProfesional> FileProfesional { get; set; }
    }
}