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
    using System.Security.Claims;

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
                FechaAlta = DateTime.Now.AddYears(-3)
            };

            manager.Create(user, "qwerty123");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "Empresa" });
                roleManager.Create(new IdentityRole { Name = "Profesional" });
            }

            var adminUser = manager.FindByName("Administrador");

            manager.AddToRoles(adminUser.Id, new string[] { "Admin" });
            manager.AddClaim(adminUser.Id, new Claim("adminId", adminUser.Id.ToString()));
            manager.AddClaim(adminUser.Id, new Claim("app_usertype", "administrador"));
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

            #region fpaz: Semilla para Tipos de Identificacion de Empresas
            var listTiposIdEmp = new List<TipoIdentificacionEmpresa>{
                new TipoIdentificacionEmpresa {Nombre="CUIT", Descripcion=" Clave Única de Identificación Tributaria "}
            };
            context.TiposIdentificacionesEmpresas.AddRange(listTiposIdEmp);
            #endregion

            #region fpaz: Semilla para Cargar un Profesional por defecto (Solo Para Desarrollo)
            var prof = new Profesional
            {
                Nombre = "Nombre Profesional 1",
                Apellido = "Apellidod Profesional 1",
                FechaNac = new DateTime(2016, 4, 30)
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
                new TipoRequisito{Nombre="Edad", Verificable=true, Habilitado =  true},
                new TipoRequisito{Nombre="Sexo",Verificable=true, Habilitado =  true},
                new TipoRequisito{Nombre="Lugar de Residencia",Verificable=false, Habilitado=true},
                new TipoRequisito{Nombre="Identidad",Verificable=true, Habilitado =  true},
                new TipoRequisito{Nombre="Idiomas",Verificable=true, Habilitado=false}
            };
            context.TipoRequisitoes.AddRange(TipoRequisito);
            #endregion


            context.SaveChanges(); //sluna: guardo los cambios para poder usar los datos en la próxima semilla

            #region sluna: Semilla para ValoresTipoRequisito
            var valoresTipoRequisito = new List<ValoresTipoRequisito>{

                #region TipoRequisito Edad
                 new ValoresTipoRequisito{Valor ="Mayores de 40 años", Desde = 40, TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Edad")).Id},
		         new ValoresTipoRequisito{Valor ="Mayores de 30 años", Desde = 30, TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Edad")).Id},
                 new ValoresTipoRequisito{Valor ="Mayores de 18 años", Desde = 18, TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Edad")).Id},
                 new ValoresTipoRequisito{Valor ="Menores de 25 años", Desde = 18, Hasta=25, TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Edad")).Id},
                 new ValoresTipoRequisito{Valor ="Menores de 30 años", Desde = 18, Hasta=30, TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Edad")).Id},
                 new ValoresTipoRequisito{Valor ="Menores de 40 años", Desde = 18, Hasta=40, TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Edad")).Id},
	            #endregion

                 #region TipoRequisito Sexo
                 new ValoresTipoRequisito{Valor ="Masculino", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Sexo")).Id},
		         new ValoresTipoRequisito{Valor ="Femenino", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Sexo")).Id},
                 new ValoresTipoRequisito{Valor ="Otro", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Sexo")).Id},
	            #endregion

                #region TipoRequisito Lugar de Residencia
                 new ValoresTipoRequisito{Valor ="Argentina", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Lugar de Residencia")).Id},
                 new ValoresTipoRequisito{Valor ="Brasil", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Lugar de Residencia")).Id},
                 new ValoresTipoRequisito{Valor ="Resto de Latinoamérica", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Lugar de Residencia")).Id},
                 new ValoresTipoRequisito{Valor ="Indiferente", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Lugar de Residencia")).Id},
                #endregion

                #region TipoRequisito Identidad
                 new ValoresTipoRequisito{Valor ="Verificada", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Identidad")).Id},
		         new ValoresTipoRequisito{Valor ="Sin verificar", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Identidad")).Id},
	            #endregion

                #region TipoRequisito Idiomas
                 new ValoresTipoRequisito{Valor ="Español", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Idioma")).Id},
		         new ValoresTipoRequisito{Valor ="Inglés", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Idioma")).Id},
                 new ValoresTipoRequisito{Valor ="Portugués", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Idioma")).Id},
                 new ValoresTipoRequisito{Valor ="Alemán", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Idioma")).Id},
                 new ValoresTipoRequisito{Valor ="Francés", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Idioma")).Id},
                 new ValoresTipoRequisito{Valor ="Italiano", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Idioma")).Id},
                 new ValoresTipoRequisito{Valor ="Chino", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Idioma")).Id}
	            #endregion
            };
            context.ValoresTipoRequisitos.AddRange(valoresTipoRequisito);
            #endregion

            #region iafar: Semilla de TipoEtapas
            var listaTipoEtapas = new List<TipoEtapa>{
                new TipoEtapa {Nombre="Reclutamiento", Descripcion="Publicación de la oferta de empleo por parte de la Empresa"},
                new TipoEtapa {Nombre="Preseleccion", Descripcion="Escogerá aquellos que le parecen cumplen mejor con el perfil"},
                new TipoEtapa {Nombre="Entrevista Formal", Descripcion="Conversación oral y directa con el candidato, la cual busca conocer un poco el comportamiento social del individuo, así como sus capacidades sobre el área que le ocuparía en la Empresa"},
                new TipoEtapa {Nombre="Evaluacion Medica", Descripcion="Con el fin de poder investigar sobre la salud y hábitos del candidato"},
                new TipoEtapa {Nombre="Evaluacion Tecnica", Descripcion="Con el fin de poder verificar los conocimientos del candidato"},
                new TipoEtapa {Nombre="Entrevista Final", Descripcion="Con el objetivo de medir la afinidad que puede existir en la relación de trabajo y el area laboral"},
                new TipoEtapa {Nombre="Contratacion", Descripcion="Marco legal a la relación laboral entre empleado y empresa", EsFinal=true },
            };
            context.TiposEtapas.AddRange(listaTipoEtapas);
            #endregion

            #region fpaz: Semilla para Cargar una Empresa por defecto (Solo Para Desarrollo)
            var emp = new Empresa
            {
                RazonSocial = "Empresa 1 Srl",
                NombreFantasia = "Empresa de Fantasia"
            };
            context.Empresas.Add(emp);
            #endregion

            #region SLuna: Semilla para Cargar una Ofertas por defecto (Solo Para Desarrollo)
            var listOfertas = new List<Oferta>
            {
                new Oferta
                {
                    Nombre = "DISEÑO GRAFICO Y EDICIÓN DE IMAGEN.",FechaInicioConvocatoria = new DateTime(2016,1,1) ,FechaFinConvocatoria = new DateTime(2016,12,31) ,Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1
                },
                new Oferta
                {
                    Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT.",FechaInicioConvocatoria = new DateTime(2016,1,1),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1
                },
                //new Oferta {Nombre = "DESARROLLO DE APLICACIONES ANDROID.",FechaInicioConvocatoria = new DateTime(2016,1,1),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "JAVASCRIPT Y MANEJO DE NODE JS.",FechaInicioConvocatoria = new DateTime(2016,1,1),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT.",FechaInicioConvocatoria = new DateTime(2016,1,1),FechaFinConvocatoria = new DateTime(2015,12,31),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "DISEÑO GRAFICO Y EDICIÓN DE IMAGEN 2.",FechaInicioConvocatoria = new DateTime(2015,1,1),FechaFinConvocatoria = new DateTime(2015,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 2.",FechaInicioConvocatoria = new DateTime(2015,1,1),FechaFinConvocatoria = new DateTime(2015,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "DESARROLLO DE APLICACIONES ANDROID 2.",FechaInicioConvocatoria = new DateTime(2015,1,1),FechaFinConvocatoria = new DateTime(2015,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "JAVASCRIPT Y MANEJO DE NODE JS 2.",FechaInicioConvocatoria = new DateTime(2015,1,1), FechaFinConvocatoria = new DateTime(2015,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 2.",FechaInicioConvocatoria = new DateTime(2015,1,1),FechaFinConvocatoria = new DateTime(2015,12,31),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "DISEÑO GRAFICO Y EDICIÓN DE IMAGEN 3.",FechaInicioConvocatoria = new DateTime(2016,8,1),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 3.",FechaInicioConvocatoria = new DateTime(2016,8,1),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "DESARROLLO DE APLICACIONES ANDROID 3.",FechaInicioConvocatoria = new DateTime(2016,8,1),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "JAVASCRIPT Y MANEJO DE NODE JS 3.",FechaInicioConvocatoria = new DateTime(2016,8,1),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 3.",FechaInicioConvocatoria = new DateTime(2016,8,1),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "DISEÑO GRAFICO Y EDICIÓN DE IMAGEN 4.",FechaInicioConvocatoria = new DateTime(2016,4,30),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 4.",FechaInicioConvocatoria = new DateTime(2016,4,30),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "DESARROLLO DE APLICACIONES ANDROID 4.",FechaInicioConvocatoria = new DateTime(2016,4,30),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "JAVASCRIPT Y MANEJO DE NODE JS 4.",FechaInicioConvocatoria = new DateTime(2016,4,30),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = true,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1},
                //new Oferta {Nombre = "MARKETING DIGITAL, SOCIAL MANAGMENT 4.",FechaInicioConvocatoria = new DateTime(2016,4,30),FechaFinConvocatoria = new DateTime(2016,12,31),Publica = false,Descripcion = "Insertar en esta seccion una breve descripción de la oferta laboral disponible, en la que se especifica la area en la que se desempeñara el posible futuro empleado que aplique para la misma...",EmpresaId = 1}
            };
            context.Ofertas.AddRange(listOfertas);
            #endregion

            base.Seed(context);
        }
    }
}
