using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using VLaboralApi.ClasesAuxiliares;
using VLaboralApi.Hubs;
using VLaboralApi.Models;
using VLaboralApi.Providers;
using VLaboralApi.Services;
using VLaboralApi.ViewModels.Filtros;
using VLaboralApi.ViewModels.Ofertas;


namespace VLaboralApi.Controllers
{
    public class OfertasController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();


        private IQueryable<Oferta> OfertasActivas() //sluna: Entendemos como activas, las que se encuentras vigentes segun sus fechas y estan en etapaInicial.
        {
            if (User != null && User.Identity.GetUserId() != null)
            {
                //fpaz: para los usuario logueados
                var tipoUsuario = Utiles.GetTipoUsuario(User.Identity.GetUserId());

                if (tipoUsuario == Utiles.TiposUsuario.profesional)
                {
                    //fpaz: si el usuario logueado es un profesional solo muestro las ofertas publicas
                    return db.Ofertas
                    .Where(o => DbFunctions.TruncateTime(o.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now)
                                && DbFunctions.TruncateTime(o.FechaFinConvocatoria) >= DbFunctions.TruncateTime(DateTime.Now)
                        //&& o.Publica sluna: esta condicion de responder a filtros. No debe ir aqui
                                && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id);
                }
                else
                {//fpaz: si el usuario logueado es una empresa solo muestro las ofertas publicas y las privadas que haya creado la empresa

                    //fpaz: obtengo el id de la empresa
                    var empresaId = Utiles.GetReceptorId(tipoUsuario, User.Identity.GetUserId());
                    if (empresaId == null) return null;

                    return db.Ofertas
                    .Where(o => DbFunctions.TruncateTime(o.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now)
                                && DbFunctions.TruncateTime(o.FechaFinConvocatoria) >= DbFunctions.TruncateTime(DateTime.Now)
                                && o.EmpresaId == empresaId
                                && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id);
                }
            }
            else
            {
                //fpaz: para los invitados
                return db.Ofertas
                    .Where(o => DbFunctions.TruncateTime(o.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now)
                                && DbFunctions.TruncateTime(o.FechaFinConvocatoria) >= DbFunctions.TruncateTime(DateTime.Now)
                        //&& o.Publica sluna: esta condicion de responder a filtros. No debe ir aqui
                                && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id);
            }


        }


        // GET: api/Ofertas
        public IQueryable<Oferta> GetOfertas()
        {
            return db.Ofertas;
        }

        // GET: api/Ofertas?page=4&rows=50
        [ResponseType(typeof(CustomPaginateResult<Oferta>))]
        public IHttpActionResult GetOfertas(int page, int rows)
        {
            try
            {
                var data = Utiles.Paginate(new PaginateQueryParameters(page, rows)
                    , OfertasActivas()
                    , order => order.OrderBy(c => c.Id));
                return Ok(data);
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }
        }

        //GET: api/OfertasPrivadas?page=4&rows=50
        [Route("api/Ofertas/OfertasPrivadas")]
        [ResponseType(typeof(CustomPaginateResult<Oferta>))]
        public IHttpActionResult GetOfertasPrivadas(int page, int rows)
        {
            try
            {
                var tipoUsuario = Utiles.GetTipoUsuario(User.Identity.GetUserId());
                IQueryable<Oferta> query = null;

                switch (tipoUsuario)
                {
                    case Utiles.TiposUsuario.empresa:
                        {
                            var idEmpresa = Utiles.GetEmpresaId(User.Identity.GetUserId());

                            if (idEmpresa != null)
                            {
                                query = db.Ofertas.Where(o => DbFunctions.TruncateTime(o.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(o.FechaFinConvocatoria) >= DbFunctions.TruncateTime(DateTime.Now) && !o.Publica && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id && o.EmpresaId == idEmpresa);
                            }
                            break;
                        }

                    case Utiles.TiposUsuario.profesional:
                        {
                            var idProfesional = Utiles.GetProfesionalId(User.Identity.GetUserId());

                            if (idProfesional != null)
                            {
                                query = db.Ofertas.Where(o => DbFunctions.TruncateTime(o.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(o.FechaFinConvocatoria) >= DbFunctions.TruncateTime(DateTime.Now) && !o.Publica && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id && db.Notificaciones.OfType<NotificacionInvitacionOferta>().Any(n => n.OfertaId == o.Id && n.ReceptorId == idProfesional));
                            }
                            break;
                        }

                    case Utiles.TiposUsuario.administracion:
                        {
                            query = db.Ofertas.Where(o => DbFunctions.TruncateTime(o.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(o.FechaFinConvocatoria) >= DbFunctions.TruncateTime(DateTime.Now) && !o.Publica && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id);
                            break;
                        }
                }

                var data = Utiles.Paginate(new PaginateQueryParameters(page, rows), query, order => order.OrderBy(c => c.Id));
                return Ok(data);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Ofertas/5
        [ResponseType(typeof(Oferta))]
        public IHttpActionResult GetOferta(int id)
        {
            var oferta = (from o in db.Ofertas where o.Id == id select o).Include(e => e.Empresa).Include(p => p.Puestos).Include(p => p.Puestos.Select(r => r.Requisitos)).Include(p => p.Puestos.Select(r => r.Requisitos.Select(tr => tr.TipoRequisito))).Include(p => p.Puestos.Select(sr => sr.Subrubros)).Include(p => p.Puestos.Select(tc => tc.TipoContrato)).Include(p => p.Puestos.Select(d => d.Disponibilidad)).Include(et => et.EtapasOferta).Include(et => et.EtapasOferta.Select(te => te.TipoEtapa)).FirstOrDefault();


            //Oferta oferta = db.Ofertas.Find(id);
            if (oferta == null)
            {
                return NotFound();
            }

            return Ok(oferta);
        }

        // GET: api/Ofertas/5        
        public IHttpActionResult GetOfertas(int prmIdProfesional) //fpaz: funcion para obtener las ofertas relacionadas a los subrubros del empleado
        {
            try
            {
                //fpaz: obtengo los datos del profesional incluyendo los subrubros
                var prof = (from p in db.Profesionals where p.Id == prmIdProfesional select p).Include(s => s.Subrubros).FirstOrDefault();

                //fpaz: armo un array solo con los Ids de los subrubros, sirve para el where en las ofertas
                var subs = (from s in prof.Subrubros select s.Id).ToList();

                var listOfertas = new List<Oferta>();
                if (subs.Count > 0)
                {
                    //fpaz: obtengo el listado de ofertas que tengan al menos un puesto con algun subrubro cargado por el empleado
                    listOfertas = (from p in db.Puestos
                                   join o in db.Ofertas on p.OfertaId equals o.Id
                                   where
                                       //DateTime.Parse(o.FechaFinConvocatoria).CompareTo(DateTime.Now) > 0  && 
                                       p.Subrubros.Any(s => subs.Contains(s.Id)) // consulto si existe algunos de los subrubros de los puestos que este contenido dentro del array de Ids de Subrubros del Empleado
                                   select o).Take(10) //fpaz: cantidad de ofertas a devolver
                        .ToList();
                }
                else
                {
                    //fpaz: si el empleado no tiene cargado ningun subrubro, se devuelve las ultimas x ofertas
                    listOfertas = db.Ofertas.Take(1) //fpaz: cantidad de ofertas a devolver
                        .ToList();
                }
                return Ok(listOfertas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Ofertas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOferta(int id, Oferta oferta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != oferta.Id)
            {
                return BadRequest();
            }

            db.Entry(oferta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfertaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Ofertas
        [ResponseType(typeof(Oferta))]
        public IHttpActionResult PostOferta(Oferta oferta)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //oferta.FechaInicioConvocatoria = DbFunctions.TruncateTime(oferta.FechaInicioConvocatoria);
                //oferta.FechaFinConvocatoria = DbFunctions.TruncateTime(oferta.FechaFinConvocatoria);
                ConfigurarPuestos(oferta);
                db.Ofertas.Add(oferta);
                //hasta aqui guardo los datos de la oferta y sus etapas pero sin ids de etapas anteriores o siguientes y sin puestos por cada etapa
                db.SaveChanges();

                ConfigurarEtapas(oferta);
                db.SaveChanges(); //fpaz: guardo las etapas de la oferta completas

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/OfertasPrivadas
        [Route("api/OfertasPrivadas")]
        public IHttpActionResult PostOfertaPrivada(ofertaConInv ofertaPrivada)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //ofertaPrivada.oferta.FechaInicioConvocatoria = DbFunctions.TruncateTime(ofertaPrivada.oferta.FechaInicioConvocatoria);
                //ofertaPrivada.oferta.FechaFinConvocatoria = DbFunctions.TruncateTime(ofertaPrivada.oferta.FechaFinConvocatoria);
                ConfigurarPuestos(ofertaPrivada.oferta);
                db.Ofertas.Add(ofertaPrivada.oferta);
                //hasta aqui guardo los datos de la oferta y sus etapas pero sin ids de etapas anteriores o siguientes y sin puestos por cada etapa
                db.SaveChanges();

                ConfigurarEtapas(ofertaPrivada.oferta);
                db.SaveChanges(); //fpaz: guardo las etapas de la oferta completas

                #region genero las invitaciones para los profesionales en caso de ser una oferta privada

                var notificacionHelper = new NotificacionesHelper();

                var invitaciones = notificacionHelper.GenerarNotificacionesInvitacionesOferta(ofertaPrivada.oferta.Id, ofertaPrivada.profesionales);

                #endregion

                return Ok(invitaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void ConfigurarPuestos(Oferta oferta)
        {
            foreach (var puesto in oferta.Puestos)
            {
                puesto.Domicilio = null; //sluna: null hasta que definamos bien esto.
                puesto.DomicilioId = null; //sluna: null hasta que definamos bien esto.

                puesto.Subrubros = ObtenerSubRubrosPuesto(puesto.Subrubros);
            }
        }

        private List<SubRubro> ObtenerSubRubrosPuesto(IEnumerable<SubRubro> subRubros)
        {
            var subrubrosPuesto = new List<SubRubro>();
            foreach (var subRubro in subRubros.ToList())
            {
                var a = db.SubRubros.Find(subRubro.Id);
                //obtengo el objeto subrubro (esto por que es una relacion M a M)

                subrubrosPuesto.Add(a);
                //agrego el subrubro al array de subrubros del profesional                                                
            }

            return subrubrosPuesto;
            //return subRubros.ToList().Select(subRubro => db.SubRubros.Find(subRubro.Id)).ToList();
        }

        private static void ConfigurarEtapas(Oferta oferta)
        {
            if (oferta.EtapasOferta == null) return;

            //fpaz: carga de etapas de una oferta
            foreach (var etapa in oferta.EtapasOferta)
            {
                OrdenarEtapas(oferta, etapa);
                ConfigurarPuestosEtapa(oferta, etapa);
            }
        }

        private static void OrdenarEtapas(Oferta oferta, EtapaOferta etapa)
        {
            #region fpaz defino los id de etapa anterior y siguiente para cada etapa

            if (etapa.Orden == 0)
            {
                //fpaz: si el orden es 0 es la etapa inicial 
                etapa.IdEtapaAnterior = 0;
                etapa.IdEstapaSiguiente = (from e in oferta.EtapasOferta where e.Orden == etapa.Orden + 1 select e.Id).FirstOrDefault();
                oferta.IdEtapaActual = etapa.Id;
            }
            else
            {
                if (etapa.Orden == oferta.EtapasOferta.Count)
                {
                    //fpaz: es la ultima etapa
                    etapa.IdEtapaAnterior = (from e in oferta.EtapasOferta where e.Orden == etapa.Orden - 1 select e.Id).FirstOrDefault();
                    etapa.IdEstapaSiguiente = 0;
                }
                else
                {
                    //fpaz: es alguna etapa intermedia
                    etapa.IdEtapaAnterior = (from e in oferta.EtapasOferta where e.Orden == etapa.Orden - 1 select e.Id).FirstOrDefault();
                    etapa.IdEstapaSiguiente = (from e in oferta.EtapasOferta where e.Orden == etapa.Orden + 1 select e.Id).FirstOrDefault();
                }
            }

            #endregion
        }

        private static void ConfigurarPuestosEtapa(Oferta oferta, EtapaOferta etapa)
        {
            #region fpaz defino los puestos para cada etapa

            var listPuestosEtapa = new List<PuestoEtapaOferta> { };
            foreach (var puesto in oferta.Puestos)
            {
                var p = new PuestoEtapaOferta
                {
                    Puesto = puesto
                };

                listPuestosEtapa.Add(p);
            }
            etapa.PuestosEtapaOferta = listPuestosEtapa;

            #endregion
        }

        // DELETE: api/Ofertas/5
        [ResponseType(typeof(Oferta))]
        public IHttpActionResult DeleteOferta(int id)
        {
            Oferta oferta = db.Ofertas.Find(id);
            if (oferta == null)
            {
                return NotFound();
            }

            db.Ofertas.Remove(oferta);
            db.SaveChanges();

            return Ok(oferta);
        }


        [Route("api/Ofertas/PasarSiguienteEtapa/{id}")]
        [ResponseType(typeof(Postulacion))]
        public IHttpActionResult PostOfertaPasarSiguienteEtapa(int id)
        {
            //Sluna: Obtengo la etapa actual a partir de IdEtapaActual de la Oferta. Joineo PuestosEtapaOferta y Postulaciones.
            var etapaActual = db.EtapasOfertas.Include(eo => eo.PuestosEtapaOferta.Select(peo => peo.Postulaciones)).FirstOrDefault(eo => eo.Id == eo.Oferta.IdEtapaActual && eo.Oferta.Id == id);


            if (etapaActual == null) return BadRequest("Ocurrió un error al buscar la etapa actual.");
            {
                if (etapaActual.IdEstapaSiguiente == 0)
                {
                    return BadRequest("La oferta no se puede avanzar de Etapa porque la Oferta ya se encuentra en la última etapa.");
                }

                var puestosEtapaOferta = etapaActual.PuestosEtapaOferta;

                //Sluna: Obtengo la etapa Siguiente a partir de IdEstapaSiguiente de la EtapaActual. Joineo PuestosEtapaOferta.
                var etapaSiguiente = db.EtapasOfertas.Include(eo => eo.PuestosEtapaOferta).FirstOrDefault(eo => eo.Id == etapaActual.IdEstapaSiguiente && eo.Oferta.Id == id);

                if (etapaSiguiente == null) return BadRequest("Ocurrío un error al buscar la etapa siguiente.");

                //Inicio la transacción
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Sluna: Paso las postulaciones marcadas con "PasaEtapa" de la EtapaActual a cada uno de los PuestosEtapaOferta de la EtapaSiguiente.
                        foreach (var puestoEtapaOferta in puestosEtapaOferta)
                        {
                            var puestoEtapaSiguiente = etapaSiguiente.PuestosEtapaOferta.FirstOrDefault(peo => peo.PuestoId == puestoEtapaOferta.PuestoId);

                            if (puestoEtapaSiguiente == null)
                            {
                                dbTransaction.Rollback();
                                return BadRequest("La Etapa Siguiente no tiene puestos configurados.");
                            }

                            var postulantesAprobados = puestoEtapaOferta.Postulaciones.Where(p => p.PasaEtapa).Select(postulante => new Postulacion
                            {
                                ProfesionalId = postulante.ProfesionalId,
                                Fecha = DateTime.Now,
                                PuestoEtapaOfertaId = puestoEtapaSiguiente.Id
                            }).ToList();

                            db.Postulacions.AddRange(postulantesAprobados);
                        }

                        //Sluna: Pongo fechaFin a la etapa actual
                        etapaActual.FechaFin = DateTime.Now;

                        //Sluna: Actualizo el IdEtapaActual de la Oferta
                        db.Ofertas.FirstOrDefault(o => o.Id == id).IdEtapaActual = etapaSiguiente.Id;

                        db.SaveChanges();

                        //Cierro la transacción
                        dbTransaction.Commit();

                        var notificacionHelper = new NotificacionesHelper();
                        var notificaciones = notificacionHelper.GenerarNotificacionesPostulantesPasanEtapa(id);

                        var notificacionesHub = new NotificacionesHub();
                        foreach (var notificacion in notificaciones)
                        {
                            notificacionesHub.EnviarNotificacionPostulanteEtapaAprobada(notificacion);
                        }
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        //Aborto la transacción
                        dbTransaction.Rollback();
                        BadRequest(ex.Message);
                    }
                }
            }
            return BadRequest();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OfertaExists(int id)
        {
            return db.Ofertas.Count(e => e.Id == id) > 0;
        }

        [HttpPost]
        [Route("api/Ofertas/QueryOptions")]
        public IHttpActionResult QueryOptions(OfertasOptionsBindingModel options)
        {
            dynamic filters = new JObject();

            if (options != null && options.Filters != null)
            {
                //the filter values should be unique 'display' strings 
                if (options.Filters.Contains(OfertasFilterOptions.Rubros))
                {
                    //filters.Rubros = new List<ValorFiltroViewModel>();
                    filters.Rubros = JArray.FromObject(
                         db.SubRubros.Where(
                             s =>
                                 s.Puestos.Any(
                                     p =>
                                         DbFunctions.TruncateTime(p.Oferta.FechaInicioConvocatoria) <=
                                         DbFunctions.TruncateTime(DateTime.Now) &&
                                         DbFunctions.TruncateTime(p.Oferta.FechaFinConvocatoria) >=
                                         DbFunctions.TruncateTime(DateTime.Now) && p.Oferta.Publica &&
                                         p.Oferta.IdEtapaActual ==
                                         p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id))
                             .Select(r => new ValorFiltroViewModel()
                             {
                                 Id = r.Id,
                                 Valor = r.Id.ToString(),
                                 Descripcion = r.Nombre,
                                 Cantidad =
                                     db.Puestos.Count(
                                         p =>
                                             p.Subrubros.Any(s => s.Id == r.Id) &&
                                             DbFunctions.TruncateTime(p.Oferta.FechaInicioConvocatoria) <=
                                             DbFunctions.TruncateTime(DateTime.Now) &&
                                             DbFunctions.TruncateTime(p.Oferta.FechaFinConvocatoria) >=
                                             DbFunctions.TruncateTime(DateTime.Now) && p.Oferta.Publica &&
                                             p.Oferta.IdEtapaActual ==
                                             p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)
                             }).ToList());
                }
                if (options.Filters.Contains(OfertasFilterOptions.DisponibilidadHoraria))
                {
                    filters.DisponibilidadHoraria =  JArray.FromObject(db.TipoDisponibilidads.Where(d => d.Puestos.Select(p => p.Oferta).Any(o => DbFunctions.TruncateTime(o.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(o.FechaFinConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && o.Publica && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)).Select(d => new ValorFiltroViewModel()
                    {
                        Id = d.Id,
                        Valor = d.Id.ToString(),
                        Descripcion = d.Nombre,
                        Cantidad = db.Puestos.Count(p => p.TipoDisponibilidadId == d.Id && DbFunctions.TruncateTime(p.Oferta.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(p.Oferta.FechaFinConvocatoria) >= DbFunctions.TruncateTime(DateTime.Now) && p.Oferta.Publica && p.Oferta.IdEtapaActual == p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)
                    }).ToList());
                }
                if (options.Filters.Contains(OfertasFilterOptions.TipoContratacion))
                {
                    filters.TipoContratacion = JArray.FromObject( db.TipoContratoes.Where(d => d.Puestos.Select(p => p.Oferta).Any(o => DbFunctions.TruncateTime(o.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(o.FechaFinConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && o.Publica && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)).Select(d => new ValorFiltroViewModel()
                    {
                        Id = d.Id,
                        Valor = d.Id.ToString(),
                        Descripcion = d.Nombre,
                        Cantidad = db.Puestos.Count(p => p.TipoContratoId == d.Id && DbFunctions.TruncateTime(p.Oferta.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(p.Oferta.FechaFinConvocatoria) >= DbFunctions.TruncateTime(DateTime.Now) && p.Oferta.Publica && p.Oferta.IdEtapaActual == p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)
                    }).ToList());
                }
                if (options.Filters.Contains(OfertasFilterOptions.Ubicaciones))
                {
                    filters.Ubicaciones =  JArray.FromObject(db.Ciudades.Where(c => c.Domicilios.Any(d => d.Puestos.Any(p => DbFunctions.TruncateTime(p.Oferta.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(p.Oferta.FechaFinConvocatoria) >= DbFunctions.TruncateTime(DateTime.Now) && p.Oferta.Publica && p.Oferta.IdEtapaActual == p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id))).Select(c => new ValorFiltroViewModel()
                    {
                        Id = c.Id,
                        Valor = c.Id.ToString(),
                        Descripcion = c.Nombre,
                        Cantidad = db.Puestos.Count(p => p.Domicilio.CiudadId == c.Id && DbFunctions.TruncateTime(p.Oferta.FechaInicioConvocatoria) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(p.Oferta.FechaFinConvocatoria) >= DbFunctions.TruncateTime(DateTime.Now) && p.Oferta.Publica && p.Oferta.IdEtapaActual == p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)
                    }).ToList());
                }

                //sluna: Ofrezco las opciones de TiposOferta (privadas o publicas) solo si es empresa.
                if (User != null)
                {
                    var tipoUsuario = Utiles.GetTipoUsuario(User.Identity.GetUserId());
                    if (tipoUsuario == Utiles.TiposUsuario.empresa)
                    {
                        if (options.Filters.Contains(OfertasFilterOptions.TiposOferta))
                        {
                            //sluna: me hubiese gustado tener los tipos en una clase y no hardcodeado en un boolean.
                            filters.TiposOferta =  JArray.FromObject(Enum.GetNames(typeof(TiposOferta)));
                        }
                    }
                }
            }

            return Ok(new
            {
                options = new
                {
                    selectableFilters = filters,
                    allFilterTypes = Enum.GetNames(typeof(OfertasFilterOptions)),
                    orderByOptions = Enum.GetNames(typeof(OfertasOrderByOptions)),
                },
                query = new
                {
                    orderBy = "",
                    searchText = "",
                    Rubros = new List<string>(),
                    DisponibilidadHoraria = new List<string>(),
                    TipoContratacion = new List<string>(),
                    Ubicacion = new List<string>(),
                    TiposOferta = new List<string>()
                }
            });
        }

        [HttpPost]
        [Route("api/Ofertas/Search")]
        public IHttpActionResult Search(OfertasQueryBindingModel queryOptions)
        {
            if (queryOptions == null)
            {
                return BadRequest("no query options provided");
            }

            //create the initial query...
            var query = OfertasActivas();


            //for each query option if it has values add it to the query
            if (!string.IsNullOrEmpty(queryOptions.SearchText))
            {
                query = query.Where(p => p.Nombre.Contains(queryOptions.SearchText) || p.Descripcion.Contains(queryOptions.SearchText));
            }

            if (queryOptions.Rubros != null && queryOptions.Rubros.Any())
            {
                query = query.Where(o => o.Puestos.Any(p => p.Subrubros.Any(s => queryOptions.Rubros.Contains(s.Id))));
            }

            if (queryOptions.DisponibilidadHoraria != null && queryOptions.DisponibilidadHoraria.Any())
            {
                query = query.Where(o => o.Puestos.Any(p => queryOptions.DisponibilidadHoraria.Contains(p.TipoDisponibilidadId)));
            }

            if (queryOptions.TipoContratacion != null && queryOptions.TipoContratacion.Any())
            {
                query = query.Where(o => o.Puestos.Any(p => queryOptions.TipoContratacion.Contains(p.TipoContratoId)));
            }

            if (queryOptions.Ubicaciones != null && queryOptions.Ubicaciones.Any())
            {
                query = query.Where(o => o.Puestos.Any(p => queryOptions.Ubicaciones.Contains((int)p.Domicilio.CiudadId)));
            }

            if (queryOptions.TiposOferta == null || !queryOptions.TiposOferta.Any())
            {
                query = query.Where(o => o.Publica);
                //sluna: por defecto, si no mandan el tipoOferta, muestro las publicas.
            }
            else
            {
                if (User == null)
                {
                    query = query.Where(o => o.Publica);
                    //sluna: por defecto, si mandan TiposOferta pero no es usuario registrado, muestro solo publicas. 
                }
                else
                {
                    //sluna: si mandan TiposOferta y ademas es usuario registrado
                    var tipoUsuario = Utiles.GetTipoUsuario(User.Identity.GetUserId());

                    //sluna: y es empresa
                    if (tipoUsuario == Utiles.TiposUsuario.empresa)
                    {
                        //sluna: es horrible esto. TODO: investigar PredicateBuilder http://www.albahari.com/nutshell/predicatebuilder.aspx
                        var publica = false;
                        var privada = false;
                        foreach (var tipoOferta in queryOptions.TiposOferta)
                        {
                            switch (tipoOferta)
                            {
                                case TiposOferta.Publica:
                                    {
                                        publica = true;
                                        break;
                                    }
                                case TiposOferta.Privada:
                                    {
                                        privada = true;
                                        break;
                                    }
                            }
                        }

                        if (publica && privada)
                        {
                            query = query.Where(o => o.Publica || !o.Publica);
                        }
                        if (publica && !privada)
                        {
                            query = query.Where(o => o.Publica);
                        }
                        if (privada && !publica)
                        {
                            query = query.Where(o => !o.Publica);
                        }
                    }
                }
            }


            query = CreateOrderByExpression(query, queryOptions.OrderBy);

            var data = Utiles.Paginate(new PaginateQueryParameters(queryOptions.Page, queryOptions.Rows), query);
            return Ok(data);
        }

        private static IQueryable<Oferta> CreateOrderByExpression(IQueryable<Oferta> query, OfertasOrderByOptions orderByoption)
        {
            switch (orderByoption)
            {
                case OfertasOrderByOptions.FechaInicioConvocatoria:
                    query = query.OrderBy(p => p.FechaInicioConvocatoria);
                    break;
                case OfertasOrderByOptions.FechaFinConvocatoria:
                    query = query.OrderByDescending(p => p.FechaFinConvocatoria);
                    break;
                default:
                    query = query.OrderBy(o => o.FechaInicioConvocatoria);
                    break;
            }

            return query;
        }
    }

    public class ofertaConInv
    {
        public Oferta oferta { get; set; }
        public ICollection<Profesional> profesionales { get; set; }
    }
}