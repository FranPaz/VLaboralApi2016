using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using VLaboralApi.ClasesAuxiliares;
using VLaboralApi.Hubs;
using VLaboralApi.Models;
using VLaboralApi.Services;
using VLaboralApi.ViewModels.Filtros;
using VLaboralApi.ViewModels.Ofertas;


namespace VLaboralApi.Controllers
{
    public class OfertasController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();


        private IQueryable<Oferta> OfertasActivas()
        {
            return db.Ofertas
                .Where(o => o.FechaInicioConvocatoria <= DateTime.Now
                            && o.FechaFinConvocatoria >= DateTime.Now
                            && o.Publica
                            && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id);
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

        // GET: api/OfertasPrivadas?page=4&rows=50
        //[Route("api/OfertasPrivadas")]
        //[ResponseType(typeof(CustomPaginateResult<Oferta>))]
        //public IHttpActionResult GetOfertasPrivadas(int page, int rows)
        //{
        //    try
        //    {
        //        return null; //sluna: falta todavia
        //        //var idProfesional = Utiles.GetProfesionalId(User.Identity.GetUserId());

        //        //var totalRows = db.Ofertas.Count(o => o.FechaInicioConvocatoria <= DateTime.Now && o.FechaFinConvocatoria >= DateTime.Now
        //        //     && !o.Publica
        //        //     && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id);

        //        ////var totalRows = 10;
        //        //var totalPages = (int)Math.Ceiling((double)totalRows / rows);
        //        //var results = db.Ofertas
        //        //    .Where(o => o.FechaInicioConvocatoria <= DateTime.Now && o.FechaFinConvocatoria >= DateTime.Now
        //        //        && o.Publica
        //        //        && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)
        //        //    .OrderBy(o => o.Id)
        //        //    .Skip((page - 1) * rows) //SLuna: -1 Para manejar indice(1) en pagina
        //        //    .Take(rows)
        //        //    .ToList();
        //        ////if (!results.Any()) { return NotFound(); } //SLuna: Si no tienes elementos devuelvo 404

        //        //var result = new CustomPaginateResult<Oferta>()
        //        //{
        //        //    PageSize = rows,
        //        //    TotalRows = totalRows,
        //        //    TotalPages = totalPages,
        //        //    CurrentPage = page,
        //        //    Results = results
        //        //};

        //        //return Ok(result);
        //    }
        //    catch (Exception ex)
        //    { return BadRequest(ex.Message); }
        //}

        // GET: api/Ofertas/5
        [ResponseType(typeof(Oferta))]
        public IHttpActionResult GetOferta(int id)
        {
            var oferta = (from o in db.Ofertas
                          where o.Id == id
                          select o)
                         .Include(e => e.Empresa)
                         .Include(p => p.Puestos)
                         .Include(p => p.Puestos.Select(r => r.Requisitos))
                         .Include(p => p.Puestos.Select(r => r.Requisitos.Select(tr => tr.TipoRequisito)))
                         .Include(p => p.Puestos.Select(sr => sr.Subrubros))
                         .Include(p => p.Puestos.Select(tc => tc.TipoContrato))
                         .Include(p => p.Puestos.Select(d => d.Disponibilidad))
                         .Include(et => et.EtapasOferta)
                         .Include(et => et.EtapasOferta.Select(te => te.TipoEtapa))
                         .FirstOrDefault();



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
                var prof = (from p in db.Profesionals
                            where p.Id == prmIdProfesional
                            select p)
                            .Include(s => s.Subrubros)
                            .FirstOrDefault();

                //fpaz: armo un array solo con los Ids de los subrubros, sirve para el where en las ofertas
                var subs = (from s in prof.Subrubros
                            select s.Id).ToList();

                var listOfertas = new List<Oferta>();
                if (subs.Count > 0)
                {
                    //fpaz: obtengo el listado de ofertas que tengan al menos un puesto con algun subrubro cargado por el empleado
                    listOfertas = (from p in db.Puestos
                                   join o in db.Ofertas
                                   on p.OfertaId equals o.Id
                                   where
                                       //DateTime.Parse(o.FechaFinConvocatoria).CompareTo(DateTime.Now) > 0  && 
                                   p.Subrubros.Any(s => subs.Contains(s.Id)) // consulto si existe algunos de los subrubros de los puestos que este contenido dentro del array de Ids de Subrubros del Empleado
                                   select o)
                             .Take(10) //fpaz: cantidad de ofertas a devolver
                             .ToList();
                }
                else
                {
                    //fpaz: si el empleado no tiene cargado ningun subrubro, se devuelve las ultimas x ofertas
                    listOfertas = db.Ofertas
                            .Take(1) //fpaz: cantidad de ofertas a devolver
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

                foreach (var puesto in oferta.Puestos)
                {
                    puesto.Domicilio = null; //sluna: null hasta que definamos bien esto.
                    puesto.DomicilioId = null; //sluna: null hasta que definamos bien esto.

                    var subrubrosPuesto = new List<SubRubro>();

                    foreach (var subRubro in puesto.Subrubros.ToList())
                    {
                        var a = db.SubRubros.Find(subRubro.Id); //obtengo el objeto subrubro (esto por que es una relacion M a M)
                        subrubrosPuesto.Add(a);//agrego el subrubro al array de subrubros del profesional                                                
                    }

                    puesto.Subrubros = subrubrosPuesto;
                }
                db.Ofertas.Add(oferta); //hasta aqui guardo los datos de la oferta y sus etapas pero sin ids de etapas anteriores o siguientes y sin puestos por cada etapa
                db.SaveChanges();

                //fpaz: carga de etapas de una oferta
                if (oferta.EtapasOferta != null)
                {
                    foreach (var etapa in oferta.EtapasOferta)
                    {
                        #region fpaz defino los id de etapa anterior y siguiente para cada etapa
                        if (etapa.Orden == 0)
                        {
                            //fpaz: si el orden es 0 es la etapa inicial 
                            etapa.IdEtapaAnterior = 0;
                            etapa.IdEstapaSiguiente = (from e in oferta.EtapasOferta
                                                       where e.Orden == etapa.Orden + 1
                                                       select e.Id).FirstOrDefault();
                            oferta.IdEtapaActual = etapa.Id;
                        }
                        else
                        {
                            if (etapa.Orden == oferta.EtapasOferta.Count)
                            {
                                //fpaz: es la ultima etapa
                                etapa.IdEtapaAnterior = (from e in oferta.EtapasOferta
                                                         where e.Orden == etapa.Orden - 1
                                                         select e.Id).FirstOrDefault();
                                etapa.IdEstapaSiguiente = 0;
                            }
                            else
                            {
                                //fpaz: es alguna etapa intermedia
                                etapa.IdEtapaAnterior = (from e in oferta.EtapasOferta
                                                         where e.Orden == etapa.Orden - 1
                                                         select e.Id).FirstOrDefault();
                                etapa.IdEstapaSiguiente = (from e in oferta.EtapasOferta
                                                           where e.Orden == etapa.Orden + 1
                                                           select e.Id).FirstOrDefault();
                            }
                        }
                        #endregion

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



                }
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

                foreach (var puesto in ofertaPrivada.oferta.Puestos)
                {
                    puesto.Domicilio = null; //sluna: null hasta que definamos bien esto.
                    puesto.DomicilioId = null; //sluna: null hasta que definamos bien esto.
                    List<SubRubro> subrubrosPuesto = new List<SubRubro>();

                    foreach (var subRubro in puesto.Subrubros.ToList())
                    {
                        var a = db.SubRubros.Find(subRubro.Id); //obtengo el objeto subrubro (esto por que es una relacion M a M)
                        subrubrosPuesto.Add(a);//agrego el subrubro al array de subrubros del profesional                                                
                    }

                    puesto.Subrubros = subrubrosPuesto;
                }
                db.Ofertas.Add(ofertaPrivada.oferta); //hasta aqui guardo los datos de la oferta y sus etapas pero sin ids de etapas anteriores o siguientes y sin puestos por cada etapa
                db.SaveChanges();

                //fpaz: carga de etapas de una oferta
                if (ofertaPrivada.oferta.EtapasOferta != null)
                {
                    foreach (var etapa in ofertaPrivada.oferta.EtapasOferta)
                    {
                        #region fpaz defino los id de etapa anterior y siguiente para cada etapa
                        if (etapa.Orden == 0)
                        {
                            //fpaz: si el orden es 0 es la etapa inicial 
                            etapa.IdEtapaAnterior = 0;
                            etapa.IdEstapaSiguiente = (from e in ofertaPrivada.oferta.EtapasOferta
                                                       where e.Orden == etapa.Orden + 1
                                                       select e.Id).FirstOrDefault();
                            ofertaPrivada.oferta.IdEtapaActual = etapa.Id;
                        }
                        else
                        {
                            if (etapa.Orden == ofertaPrivada.oferta.EtapasOferta.Count)
                            {
                                //fpaz: es la ultima etapa
                                etapa.IdEtapaAnterior = (from e in ofertaPrivada.oferta.EtapasOferta
                                                         where e.Orden == etapa.Orden - 1
                                                         select e.Id).FirstOrDefault();
                                etapa.IdEstapaSiguiente = 0;
                            }
                            else
                            {
                                //fpaz: es alguna etapa intermedia
                                etapa.IdEtapaAnterior = (from e in ofertaPrivada.oferta.EtapasOferta
                                                         where e.Orden == etapa.Orden - 1
                                                         select e.Id).FirstOrDefault();
                                etapa.IdEstapaSiguiente = (from e in ofertaPrivada.oferta.EtapasOferta
                                                           where e.Orden == etapa.Orden + 1
                                                           select e.Id).FirstOrDefault();
                            }
                        }
                        #endregion

                        #region fpaz defino los puestos para cada etapa
                        var listPuestosEtapa = new List<PuestoEtapaOferta> { };
                        foreach (var puesto in ofertaPrivada.oferta.Puestos)
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



                }
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
            var etapaActual = db.EtapasOfertas
                .Include(eo => eo.PuestosEtapaOferta
                    .Select(peo => peo.Postulaciones))
                .FirstOrDefault(eo => eo.Id == eo.Oferta.IdEtapaActual && eo.Oferta.Id == id);


            if (etapaActual == null) return BadRequest("Ocurrió un error al buscar la etapa actual.");
            {
                if (etapaActual.IdEstapaSiguiente == 0)
                {
                    return BadRequest("La oferta no se puede avanzar de Etapa porque la Oferta ya se encuentra en la última etapa.");
                }

                var puestosEtapaOferta = etapaActual.PuestosEtapaOferta;

                //Sluna: Obtengo la etapa Siguiente a partir de IdEstapaSiguiente de la EtapaActual. Joineo PuestosEtapaOferta.
                var etapaSiguiente = db.EtapasOfertas
                    .Include(eo => eo.PuestosEtapaOferta)
                    .FirstOrDefault(eo => eo.Id == etapaActual.IdEstapaSiguiente && eo.Oferta.Id == id);

                if (etapaSiguiente == null) return BadRequest("Ocurrío un error al buscar la etapa siguiente.");

                //Inicio la transacción
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Sluna: Paso las postulaciones marcadas con "PasaEtapa" de la EtapaActual a cada uno de los PuestosEtapaOferta de la EtapaSiguiente.
                        foreach (var puestoEtapaOferta in puestosEtapaOferta)
                        {
                            var puestoEtapaSiguiente = etapaSiguiente.PuestosEtapaOferta.FirstOrDefault(
                                          peo => peo.PuestoId == puestoEtapaOferta.PuestoId);

                            if (puestoEtapaSiguiente == null){
                                dbTransaction.Rollback();
                                return BadRequest("La Etapa Siguiente no tiene puestos configurados.");
                            }

                            var postulantesAprobados =
                                puestoEtapaOferta.Postulaciones.Where(p => p.PasaEtapa)
                                    .Select(postulante => new Postulacion
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
        public async Task<IHttpActionResult> QueryOptions(OfertasOptionsBindingModel options)
        {

            dynamic filters = new ExpandoObject();

            if (options != null && options.Filters != null)
            {
                //the filter values should be unique 'display' strings 
                if (options.Filters.Contains(OfertasFilterOptions.Rubros))
                {

                    filters.Rubros = await db.SubRubros
                            .Where(s => s.Puestos.Any(p => p.Oferta.FechaInicioConvocatoria <= DateTime.Now
                                        && p.Oferta.FechaFinConvocatoria >= DateTime.Now
                                        && p.Oferta.Publica
                                        && p.Oferta.IdEtapaActual == p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id))
                            .Select(r => new ValorFiltroViewModel()
                            {
                                Id = r.Id,
                                Valor= r.Id.ToString(),
                                Descripcion = r.Nombre,
                                Cantidad = db.Puestos
                                       .Count(p => p.Subrubros.Any(s => s.Id == r.Id) 
                                             && p.Oferta.FechaInicioConvocatoria <= DateTime.Now
                                             && p.Oferta.FechaFinConvocatoria >= DateTime.Now
                                             && p.Oferta.Publica
                                             && p.Oferta.IdEtapaActual == p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)
                            }).ToListAsync();
                }
                if (options.Filters.Contains(OfertasFilterOptions.DisponibilidadHoraria))
                
                {
                    filters.DisponibilidadHoraria = await db.TipoDisponibilidads
                        .Where(d => d.Puestos
                            .Select(p => p.Oferta)
                            .Any(o => o.FechaInicioConvocatoria <= DateTime.Now
                                      && o.FechaFinConvocatoria >= DateTime.Now
                                      && o.Publica
                                      && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id))
                        .Select(d => new ValorFiltroViewModel()
                        {
                            Id = d.Id,
                            Valor = d.Id.ToString(),
                            Descripcion = d.Nombre,
                            Cantidad = db.Puestos
                                        .Count(p => p.TipoDisponibilidadId == d.Id && p.Oferta.FechaInicioConvocatoria <= DateTime.Now
                                      && p.Oferta.FechaFinConvocatoria >= DateTime.Now
                                      && p.Oferta.Publica
                                      && p.Oferta.IdEtapaActual == p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)
                        }).ToListAsync();

                }
                if (options.Filters.Contains(OfertasFilterOptions.TipoContratacion))
                {
                    filters.TipoContratacion = await db.TipoContratoes
                        .Where(d => d.Puestos
                            .Select(p => p.Oferta)
                            .Any(o => o.FechaInicioConvocatoria <= DateTime.Now
                                      && o.FechaFinConvocatoria >= DateTime.Now
                                      && o.Publica
                                      && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id))
                        .Select(d => new ValorFiltroViewModel()
                        {
                            Id = d.Id,
                            Valor = d.Id.ToString(),
                            Descripcion = d.Nombre,
                            Cantidad = db.Puestos
                                        .Count(p => p.TipoContratoId == d.Id && p.Oferta.FechaInicioConvocatoria <= DateTime.Now
                                      && p.Oferta.FechaFinConvocatoria >= DateTime.Now
                                      && p.Oferta.Publica
                                      && p.Oferta.IdEtapaActual == p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)
                        }).ToListAsync();

                }
                if (options.Filters.Contains(OfertasFilterOptions.Ubicaciones))
                {
                    filters.Ubicaciones = db.Ciudades
                        .Where(c=> c.Domicilios
                            .Any(d=> d.Puestos
                                .Any(p =>p.Oferta.FechaInicioConvocatoria <= DateTime.Now
                                      && p.Oferta.FechaFinConvocatoria >= DateTime.Now
                                      && p.Oferta.Publica
                                      && p.Oferta.IdEtapaActual == p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)))
                        .Select(c => new ValorFiltroViewModel()
                        {
                            Id = c.Id,
                            Valor = c.Id.ToString(),
                            Descripcion = c.Nombre,
                            Cantidad = db.Puestos.Count(p => p.Domicilio.CiudadId == c.Id && p.Oferta.FechaInicioConvocatoria <= DateTime.Now
                                      && p.Oferta.FechaFinConvocatoria >= DateTime.Now
                                      && p.Oferta.Publica
                                      && p.Oferta.IdEtapaActual == p.Oferta.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)
                        }).ToListAsync();
                }

            }

            var orderByOptions = Enum.GetNames(typeof(OfertasOrderByOptions));

            return Ok(
                new
                {
                    options = new
                    {
                        selectableFilters = filters,
                        allFilterTypes = Enum.GetNames(typeof(OfertasFilterOptions)),
                        orderByOptions = orderByOptions,
                    },
                    query = new
                    {
                        orderBy = "",
                        searchText = "",
                        Rubros = new List<string>(),
                        DisponibilidadHoraria = new List<string>(),
                        Ubicacion = new List<string>()
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
                query = query.Where(o => o.Puestos.Any(p =>  queryOptions.DisponibilidadHoraria.Contains(p.TipoDisponibilidadId)));
            }

            if (queryOptions.TipoContratacion != null && queryOptions.TipoContratacion.Any())
            {
                query = query.Where(o => o.Puestos.Any(p => queryOptions.TipoContratacion.Contains(p.TipoContratoId)));
            }

            if (queryOptions.Ubicaciones != null && queryOptions.Ubicaciones.Any())
            {
                query = query.Where(o => o.Puestos.Any(p => queryOptions.Ubicaciones.Contains((int) p.Domicilio.CiudadId)));
            }

            query = CreateOrderByExpression(query, queryOptions.OrderBy);

            // var results = await query.ToListAsync();

            var data = Utiles.Paginate(new PaginateQueryParameters(queryOptions.Page, queryOptions.Rows), query); 
            return Ok(data);

            //return Ok(new
            //{
            //    Books = results
            //});
        }

        private IQueryable<Oferta> CreateOrderByExpression(IQueryable<Oferta> query, OfertasOrderByOptions orderByoption)
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

    public class ofertaConInv {
        public Oferta oferta { get; set; }
        public ICollection<Profesional> profesionales { get; set; }
    }
}