using System.Collections.Generic;
using VLaboralApi.Models.Ubicacion;

namespace VLaboralApi.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string NombreFantasia { get; set; }
        public string UrlImagenPerfil { get; set; }
        public string Descripcion { get; set; }

        public int? DomicilioId { get; set; }
        public virtual Domicilio Domicilio { get; set; }

        public string Telefono { get; set; }
        public string SitioWeb { get; set; }
       
        //iafar: relacion 1 a m con empresa (m)
        public virtual ICollection<Oferta> Ofertas { get; set; }

        //fpaz: relacion 1 a M con IdentificacionEmpresa (muchos). Tiene el array con todos los tipos de identificaciones de la empresa y sus valores (cuit, etc)        
        public virtual ICollection<IdentificacionEmpresa> IdentificacionesEmpresa { get; set; }

        //fpaz: relacion 1 a M con ExperienciaLaboral (muchos)
        public virtual ICollection<ExperienciaLaboral> ExperienciasLaborales { get; set; }

        //kike: relacion 1 a M con ImagenesEmpresa
        public virtual ICollection<ImagenEmpresa> ImagenesEmpresa { get; set; }        

    }
}