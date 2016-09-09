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

        public System.Data.Entity.DbSet<VLaboralApi.Models.SubRubro> SubRubros { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.TipoDisponibilidad> TipoDisponibilidads { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.TipoContrato> TipoContratoes { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.TipoIdentificacionProfesional> TiposIdentificacionesProfesionales { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.IdentificacionProfesional> IdentificacionesProfesional { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.TipoIdentificacionEmpresa> TiposIdentificacionesEmpresas { get; set; }
        public System.Data.Entity.DbSet<VLaboralApi.Models.IdentificacionEmpresa> IdentificacionesEmpresa { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.Profesional> Profesionals { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.Habilidad> Habilidads { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.Requisito> Requisitos { get; set; }
        public System.Data.Entity.DbSet<VLaboralApi.Models.ValoresRequisito> ValoresRequisitos { get; set; }
        
        public System.Data.Entity.DbSet<VLaboralApi.Models.TipoRequisito> TipoRequisitoes { get; set; }
        public System.Data.Entity.DbSet<VLaboralApi.Models.ValoresTipoRequisito> ValoresTipoRequisitos { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.Empresa> Empresas { get; set; }
        public System.Data.Entity.DbSet<VLaboralApi.Models.Puesto> Puestos { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.TipoEtapa> TiposEtapas { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.Postulacion> Postulacions { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.PuestoEtapaOferta> PuestoEtapaOfertas { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.EtapaOferta> EtapasOfertas { get; set; }


        #endregion

        public static VLaboral_Context Create()
        {
            return new VLaboral_Context();
        }

        public System.Data.Entity.DbSet<VLaboralApi.Models.TipoNivelEstudio> TipoNivelEstudios { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.Estudio> Estudios { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.Educacion> Educacions { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.ExperienciaLaboral> ExperienciaLaborals { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.Idioma> Idiomas { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.IdiomaConocido> IdiomaConocidoes { get; set; }

        public System.Data.Entity.DbSet<VLaboralApi.Models.CompetenciaIdioma> CompetenciaIdiomas { get; set; }
    }

}

