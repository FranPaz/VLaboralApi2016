namespace VLaboralApi.Migrations
{
    
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using VLaboralApi.Models;
    using VlaboralApi.Infrastructure;
    

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
                Apellido = "Apellidod Profesional 1",
                FechaNac = ("07/04/2016")
            };
            context.Profesionals.Add(prof);
            #endregion

            #region SLuna: Semilla para Rubros y SubRubros
            var listRubros = new List<Rubro>{
                    new Rubro {Nombre="Informática", Descripcion="Servicios relaciones con el area de informática", Subrubros = new List<SubRubro>{
                        new SubRubro { Nombre = "Analista Programador", Descripcion =""},
                        new SubRubro { Nombre = "Administrador IT", Descripcion =""}                        
                    }},
                    new Rubro {Nombre="Construcción", Descripcion="Servicios relaciones con el area de la construcción", Subrubros = new List<SubRubro>{
                        new SubRubro { Nombre = "Albañilería", Descripcion =""},
                        new SubRubro { Nombre = "Mampostería", Descripcion =""}                        
                    }},
                    new Rubro {Nombre="Mecánica del Automotor", Descripcion="Servicios relaciones con el area de la mecánica del automotor", Subrubros = new List<SubRubro>{
                        new SubRubro { Nombre = "Tren delantero", Descripcion =""},
                        new SubRubro { Nombre = "Frenos y Embrague", Descripcion =""}    
                    
                    }}
            };
            context.Rubroes.AddRange(listRubros);
            #endregion

            #region iafar: Semilla de Habilidades
            var listHabilidades = new List<Habilidad>{
                new Habilidad {Nombre="SQL", Descripcion="Manejo de Lenguaje SQL"},
                new Habilidad {Nombre="PHP", Descripcion="Manejo de Lenguaje PHP"},
                new Habilidad {Nombre="HTML5", Descripcion="Manejo de Lenguaje HTML5"},
                new Habilidad {Nombre="UI/UX web", Descripcion="Conocimiento de diseño web aplicando tecnicas UI/UX"}
            };
            context.Habilidads.AddRange(listHabilidades);
            #endregion

            #region iafar: Semilla para Tipo de Requisito
            var TipoRequisito = new List<TipoRequisito>{
                new TipoRequisito{Nombre="Edad"},
                new TipoRequisito{Nombre="Sexo"},
                new TipoRequisito{Nombre="Nacionalidad"},
                new TipoRequisito{Nombre="Identidad Verificada"}
            };
            
            context.TipoRequisitoes.AddRange(TipoRequisito);
            #endregion

            #region fpaz: Semilla para Cargar una Empresa por defecto (Solo Para Desarrollo)
            var emp = new Empresa
            {
                RazonSocial="Empresa 1 Srl",
                NombreFantasia="Empresa de Fantasia"                
            };
            context.Empresas.Add(emp);
            #endregion

            //#region SLuna: Semilla para Cargar una Ofertas por defecto (Solo Para Desarrollo)
            var listOfertas = new List<Oferta>
            {
                new Oferta {Nombre = "DISEÑO GRAFICO Y EDICIÓN DE IMAGEN.",FechaInicioConvocatoria = "09/04/2016" ,FechaFinConvocatoria = "31/12/2016" ,Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT.",FechaInicioConvocatoria = ("09/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "DESARROLLO DE APLICACIONES ANDROID.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "JAVASCRIPT Y MANEJO DE NODE JS.",FechaInicioConvocatoria = ("01/01/2015"),FechaFinConvocatoria = ("31/12/2015"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "DISEÑO GRAFICO Y EDICIÓN DE IMAGEN 2.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 2.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "DESARROLLO DE APLICACIONES ANDROID 2.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "JAVASCRIPT Y MANEJO DE NODE JS 2.",FechaInicioConvocatoria = ("07/04/2016"), FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 2.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "DISEÑO GRAFICO Y EDICIÓN DE IMAGEN 3.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 3.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "DESARROLLO DE APLICACIONES ANDROID 3.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "JAVASCRIPT Y MANEJO DE NODE JS 3.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 3.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "DISEÑO GRAFICO Y EDICIÓN DE IMAGEN 4.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 4.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "DESARROLLO DE APLICACIONES ANDROID 4.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "JAVASCRIPT Y MANEJO DE NODE JS 4.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 4.",FechaInicioConvocatoria = ("07/04/2016"),FechaFinConvocatoria = ("31/12/2016"),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1}
            };
            context.Ofertas.AddRange(listOfertas);
            //#endregion

            base.Seed(context);
        }
    }
}
