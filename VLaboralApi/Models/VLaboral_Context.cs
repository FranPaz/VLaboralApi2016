using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using VlaboralApi.Infrastructure;

namespace VLaboralApi.Models
{
    public class VLaboral_Context : IdentityDbContext<ApplicationUser> // DbContext
    {
        public VLaboral_Context() : base("VLaboral_Context", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        #region Definicion de Tablas DbSet
        public System.Data.Entity.DbSet<VLaboralApi.Models.BlobUploadModel> BlobUploadModels { get; set; }
        public System.Data.Entity.DbSet<VLaboralApi.Models.Oferta> Ofertas { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.Rubro> Rubroes { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.TipoDisponibilidad> TipoDisponibilidads { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.TipoContrato> TipoContratoes { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.TipoIdentificacionProfesional> TiposIdentificacionesProfesionales { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.Profesional> Profesionals { get; set; }      
        #endregion

        public static VLaboral_Context Create()
        {
            return new VLaboral_Context();
        }
    }

}

