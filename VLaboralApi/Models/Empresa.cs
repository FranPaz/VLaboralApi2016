using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string NombreFantasia { get; set; }
        public string UrlImagenPerfil { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string SitioWeb { get; set; }
       
        //iafar: relacion 1 a m con empresa (m)
        public virtual ICollection<Oferta> Ofertas { get; set; }

        //fpaz: relacion 1 a M con IdentificacionEmpresa (muchos). Tiene el array con todos los tipos de identificaciones de la empresa y sus valores (cuit, etc)        
        public virtual ICollection<IdentificacionEmpresa> IdentificacionesEmpresa { get; set; }        

    }
}