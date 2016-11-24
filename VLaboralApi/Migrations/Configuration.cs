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

            #region Carga de Profesional Y Empresas Por Defecto           

            //fpaz: doy de alta las instancias de Profesional y de Empresa que van a estar relacionada con los ususarios de la aplicacion por defecto            
            var prof = new Profesional
            {
                Nombre = "Nombre Profesional Prueba",
                Apellido = "Apellidod Profesional Prueba",
                IdentidadVerificada = true,
                Sexo = "Masculino",
                FechaNac = new DateTime(2016, 4, 30)
            };
            context.Profesionals.Add(prof);

            var emp = new Empresa
            {
                RazonSocial = "Empresa de prueba",
                NombreFantasia = "Empresa de Fantasia"
            };

            context.Empresas.Add(emp);


            #region fpaz: defino y guardo los tipos de Roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new VLaboral_Context()));
            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "Empresa" });
                roleManager.Create(new IdentityRole { Name = "Profesional" });
            }
            #endregion

            # region fpaz: defino los usuarios por defecto para profesional y para empresa
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new VLaboral_Context()));

            var listUsers = new List<ApplicationUser>{
                new ApplicationUser {UserName = "profesional@overcodesde.com", Email = "profesional@overcodesde.com",EmailConfirmed = true,FechaAlta = DateTime.Now.AddYears(-3)},
                new ApplicationUser {UserName = "empresa@overcodesde.com",Email = "empresa@overcodesde.com",EmailConfirmed = true,FechaAlta = DateTime.Now.AddYears(-3)}

            };

            foreach (var newUser in listUsers)
            {
                manager.Create(newUser, "qwerty123");
                manager.SetLockoutEnabled(newUser.Id, false);

                if (newUser.UserName == "profesional@overcodesde.com")
                {
                    manager.AddToRoles(newUser.Id, new string[] { "Profesional" });

                    var user = manager.FindByName("profesional@overcodesde.com");

                    var listClaims = new List<Claim>{
                        new Claim ("app_usertype", "profesional"),
                        new Claim("profesionalId", "1")
                    };

                    foreach (var item in listClaims)
                    {
                        manager.AddClaim(user.Id, item);
                    }
                }
                else
                {
                    manager.AddToRoles(newUser.Id, new string[] { "Empresa" });

                    var user = manager.FindByName("empresa@overcodesde.com");

                    var listClaims = new List<Claim>{
                        new Claim ("app_usertype", "empresa"),
                        new Claim("empresaId", "1")
                    };

                    foreach (var item in listClaims)
                    {
                        manager.AddClaim(user.Id, item);
                    }
                }

            }
            #endregion


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
                new TipoRequisito{Nombre="Edad", Verificable=true, Habilitado =  true, Multiple= false},
                new TipoRequisito{Nombre="Sexo",Verificable=true, Habilitado =  true, Multiple= false},
                new TipoRequisito{Nombre="Lugar de Residencia",Verificable=false, Habilitado=true, Multiple=true},
                new TipoRequisito{Nombre="Identidad",Verificable=true, Habilitado =  true, Multiple=false},
                new TipoRequisito{Nombre="Idiomas",Verificable=false, Habilitado=false, Multiple=true}
            };
            context.TipoRequisitoes.AddRange(TipoRequisito);
            #endregion

            #region fpaz: Semilla para Tipos de Nivel de Estudio
            var listTiposNivelesEstudio = new List<TipoNivelEstudio>{
                new TipoNivelEstudio {Nombre="Secundario", Descripcion="Nivel Secundario"},
                new TipoNivelEstudio {Nombre="Terciario", Descripcion="Nivel Terciario"},
                new TipoNivelEstudio {Nombre="Universitario", Descripcion="Nivel Universitario"}
            };
            context.TipoNivelEstudios.AddRange(listTiposNivelesEstudio);
            #endregion

            #region fpaz: Semilla para Idiomas
            var listIdiomas = new List<Idioma>{
                new Idioma {Nombre="Ingles"},
                new Idioma {Nombre="Portugues"},
                new Idioma {Nombre="Aleman"}
            };
            context.Idiomas.AddRange(listIdiomas);
            #endregion

            #region fpaz: Semilla para Competencias Idiomas
            var listCompIdiomas = new List<CompetenciaIdioma>{
                new CompetenciaIdioma {Nombre="Basico"},
                new CompetenciaIdioma {Nombre="Intermedio"},
                new CompetenciaIdioma {Nombre="Avanzado"}
            };
            context.CompetenciaIdiomas.AddRange(listCompIdiomas);
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
                // new ValoresTipoRequisito{Valor ="Otro", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Sexo")).Id},
	            #endregion

                #region TipoRequisito Lugar de Residencia
                 new ValoresTipoRequisito{Valor ="Argentina", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Lugar de Residencia")).Id},
                 new ValoresTipoRequisito{Valor ="Brasil", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Lugar de Residencia")).Id},
                 new ValoresTipoRequisito{Valor ="Resto de Latinoamérica", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Lugar de Residencia")).Id},
                 //new ValoresTipoRequisito{Valor ="Indiferente", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Lugar de Residencia")).Id},
                #endregion

                #region TipoRequisito Identidad
                 new ValoresTipoRequisito{Valor ="Verificada", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Identidad")).Id},
                 //new ValoresTipoRequisito{Valor ="Sin verificar", TipoRequisitoId = context.TipoRequisitoes.FirstOrDefault(t=> t.Nombre.Contains("Identidad")).Id},
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
                new TipoEtapa {Nombre="Reclutamiento", Descripcion="Publicación de la oferta de empleo por parte de la Empresa", EsInicial=true},
                new TipoEtapa {Nombre="Preseleccion", Descripcion="Escogerá aquellos que le parecen cumplen mejor con el perfil"},
                new TipoEtapa {Nombre="Entrevista Formal", Descripcion="Conversación oral y directa con el candidato, la cual busca conocer un poco el comportamiento social del individuo, así como sus capacidades sobre el área que le ocuparía en la Empresa"},
                new TipoEtapa {Nombre="Evaluacion Medica", Descripcion="Con el fin de poder investigar sobre la salud y hábitos del candidato"},
                new TipoEtapa {Nombre="Evaluacion Tecnica", Descripcion="Con el fin de poder verificar los conocimientos del candidato"},
                new TipoEtapa {Nombre="Entrevista Final", Descripcion="Con el objetivo de medir la afinidad que puede existir en la relación de trabajo y el area laboral"},
                new TipoEtapa {Nombre="Contratacion", Descripcion="Marco legal a la relación laboral entre empleado y empresa", EsFinal=true },
            };
            context.TiposEtapas.AddRange(listaTipoEtapas);
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

            #region sluna: Semilla de TipoNotificacion
            var listaTipoNotificacion = new List<TipoNotificacion>{
                new TipoNotificacion {Valor="EXP", Descripcion="Notificación de Experiencia",
                    Titulo = "Notificación de experiencia laboral", Mensaje = "sdasdasdasd.",
                    TipoEmisor = "profesional" , TipoReceptor = "empresa"},

                     new TipoNotificacion {Valor="EXPVER", Descripcion="Notificación de Experiencia",
                    Titulo = "Notificación de experiencia laboral verificada", Mensaje = "Una empresa a verificado una experiencia laboral.",
                    TipoEmisor = "empresa" , TipoReceptor = "profesional"},

                 new TipoNotificacion {Valor = "POS", Descripcion="Notificación de Postulación",
                        Titulo = "Notificación de postulación",
                        Mensaje = "Un profesional se ha postulado a un puesto de una oferta.",
                       TipoEmisor = "profesional" , TipoReceptor = "empresa"},

                 new TipoNotificacion { Valor = "ETAP" ,  Descripcion="Notificación de Etapa Aprobada",
                        Titulo = "Notificación de etapa aprobado",
                        Mensaje = "Felicitaciones, has aprobado la etapa actual.",
                         TipoEmisor = "empresa" , TipoReceptor = "profesional"},

                 new TipoNotificacion { Valor = "INV_OFER_PRIV" ,  Descripcion="Notificación de Invitacion de Oferta Privada",
                        Titulo = "Invitacion de Oferta Privada",
                        Mensaje = "Una empresa lo invito a participar de una oferta laboral",
                         TipoEmisor = "empresa" , TipoReceptor = "profesional"},
            };
            context.TipoNotificaciones.AddRange(listaTipoNotificacion);
            context.SaveChanges();
            #endregion

            base.Seed(context);
        }
    }
}
