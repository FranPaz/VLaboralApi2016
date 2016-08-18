using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VLaboralApi.Models;
using VLaboralApi.Services;

namespace VLaboralApi.Controllers
{
    public class OfertasController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

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
                //SLuna: Cuento la cantidad de Ofertas vigentes que hay cargadas.
                //Sluna: Para que sea una oferta vigente, la fecha actual tiene que estar dentro de las fechas de inicio y fin de convocatoria,
                //sluna: además, la etapaActual de la oferta tiene que ser la primera, es decir, que la etapaActual tiene que tener idEstapaAnterior = 0
                var totalRows = db.Ofertas.Count(o => o.FechaInicioConvocatoria <= DateTime.Now && o.FechaFinConvocatoria >= DateTime.Now
                    && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id);

                var totalPages = (int)Math.Ceiling((double)totalRows / rows);
                var results = db.Ofertas
                    .Where(o => o.FechaInicioConvocatoria <= DateTime.Now && o.FechaFinConvocatoria >= DateTime.Now
                    && o.IdEtapaActual == o.EtapasOferta.FirstOrDefault(e => e.TipoEtapa.EsInicial == true).Id)
                    .OrderBy(o => o.Id)
                    .Skip((page - 1) * rows) //SLuna: -1 Para manejar indice(1) en pagina
                    .Take(rows)
                    .ToList();
                if (!results.Any()) { return NotFound(); } //SLuna: Si no tienes elementos devuelvo 404

                var result = new CustomPaginateResult<Oferta>()
                {
                    PageSize = rows,
                    TotalRows = totalRows,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    Results = results
                };

                return Ok(result);
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }
        }

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
                    List<SubRubro> subrubrosPuesto = new List<SubRubro>();

                    foreach (var subRubro in puesto.Subrubros.ToList())
                    {
                        var a = db.SubRubros.Find(subRubro.Id); //obtengo el objeto subrubro (esto por que es una relacion M a M)
                        subrubrosPuesto.Add(a);//agrego el subrubro al array de subrubros del profesional                                                
                    }

                    puesto.Subrubros = subrubrosPuesto;
                }

                //fpaz: carga de etapas de una oferta
                if (oferta.EtapasOferta.Count < 1)
                {
                    //si no se cargaron etapas doy de alta las etapas por defecto
                    var etapaInicial = new EtapaOferta();
                    var etapaFinal = new EtapaOferta();

                    etapaInicial.IdEtapaAnterior = 0;
                    etapaInicial.IdEstapaSiguiente = etapaFinal.Id;
                    etapaInicial.TipoEtapaId = db.TiposEtapas.FirstOrDefault().Id;

                    etapaFinal.IdEtapaAnterior = etapaInicial.Id;
                    etapaFinal.IdEstapaSiguiente = 0;
                    etapaFinal.TipoEtapaId = db.TiposEtapas.LastOrDefault().Id;

                    var listPuestosEtapa = new List<PuestoEtapaOferta> { };
                    foreach (var puesto in oferta.Puestos)
                    {
                        var p = new PuestoEtapaOferta
                        {
                            PuestoId = puesto.Id
                        };

                        listPuestosEtapa.Add(p);
                    }

                    etapaInicial.PuestosEtapaOferta = listPuestosEtapa;
                    etapaFinal.PuestosEtapaOferta = listPuestosEtapa;

                    oferta.IdEtapaActual = etapaInicial.Id;
                    oferta.EtapasOferta.Add(etapaInicial);
                    oferta.EtapasOferta.Add(etapaFinal);
                }

                db.Ofertas.Add(oferta);
                db.SaveChanges();
                return Ok();
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
    }
}