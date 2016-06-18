namespace VLaboralApi.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using VlaboralApi.Infrastructure;
    using VLaboralApi.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<VLaboralApi.Models.VLaboral_Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VLaboralApi.Models.VLaboral_Context context)
        {
            //fpaz:Semillas para el llenado inicial de la bd
            
            #region Carga de ApplicationUser
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new VLaboral_Context()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new VLaboral_Context()));

            var user = new ApplicationUser()
            {
                UserName = "Administrador",
                Email = "overcode_dev@outlook.com",
                EmailConfirmed = true,
                FirstName = "Administrador",
                LastName = "Administrador",
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3),
                //Empleador = new Empleador { Cuit = "123", Rsocial = "Empleador 1", Descripcion = "Empleador de prueba 1" },
            };

            manager.Create(user, "qwerty123");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByName("Administrador");

            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });
            #endregion

            #region fpaz: Semilla para Tipos de Disponibilidad
            var listTiposList = new List<TipoDisponibilidad>{
                new TipoDisponibilidad {Nombre="Full-time", Descripcion="Disponibilidad a Tiempo Completo"},
                new TipoDisponibilidad {Nombre="Part-Time", Descripcion="Disponibilidad a Tiempo Parcial"}
                
            };
            context.TipoDisponibilidads.AddRange(listTiposList);
            #endregion

            #region fpaz: Semilla para Tipos de Contratos
            var listTiposContratos = new List<TipoContrato>{
                new TipoContrato {Nombre="Tiempo Indeterminado", Descripcion="Contrato por tiempo indeterminado"},
                new TipoContrato {Nombre="Tiempo Parcial", Descripcion="Contrato de trabajo a tiempo parcial"},                
                new TipoContrato {Nombre="Eventual", Descripcion="Contrato de trabajo eventual"},
                new TipoContrato {Nombre="Por Temporada", Descripcion="Contrato de trabajo de temporada"}
                
            };
            context.TipoContratoes.AddRange(listTiposContratos);
            #endregion

            #region fpaz: Semilla para Tipos de Identificacion de Profesionales
            var listTiposIdProf = new List<TipoIdentificacionProfesional>{
                new TipoIdentificacionProfesional {Nombre="DNI", Descripcion="Documento Nacional de Identidad"},
                new TipoIdentificacionProfesional {Nombre="CUIT", Descripcion=" Clave Única de Identificación Tributaria "},                
                new TipoIdentificacionProfesional {Nombre="Pasaporte", Descripcion="Pasaporte Internacional"}                
                
            };
            context.TiposIdentificacionesProfesionales.AddRange(listTiposIdProf);
            #endregion

            #region fpaz: Semilla para Cargar un Profesional por defecto (Solo Para Desarrollo)
            var prof = new Profesional
            {
                Nombre = "Nombre Profesional 1",
                Apellido = "Apellidod Profesional 1"                
            };
            context.Profesionals.Add(prof);
            #endregion

            #region kikexp: Semilla para Cargar una Habilidad por Defecto
            var Habilidad = new Habilidad
            {
                Nombre = "Habilidad Prueba",
                Descripcion = "Descripcion Habilidad Prueba"
            };
            context.Habilidads.Add(Habilidad);
            #endregion

            #region kikexp: Semilla para Tipo de Requisito
            var TipoRequisito = new TipoRequisito
            {
                Nombre = "Tipo de requisito Prueba"
            };
            context.TipoRequisitoes.Add(TipoRequisito);
            #endregion

            base.Seed(context);
        }
    }
}
