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

namespace VLaboralApi.Controllers
{
    public class EtapaOfertasController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/EtapaOfertas
        public IHttpActionResult GetEtapasOfertas() //fpaz: funcion que devuelve las etapas por defecto de cada oferta (etapa inicial y final)
        {
            try
            {
                List<EtapaOferta> listEtapasObligatorias = new List<EtapaOferta>();

                var etapaInicial = new EtapaOferta();
                var etapaFinal = new EtapaOferta();


                etapaInicial.Orden = 0;
                etapaInicial.TipoEtapa = (from te in db.TiposEtapas
                                          select te).FirstOrDefault();



                etapaFinal.Orden = 1;
                var idUltimaEtapa = db.TiposEtapas.Max(p => p.Id);
                etapaFinal.TipoEtapa = db.TiposEtapas.Find(idUltimaEtapa);

                listEtapasObligatorias.Add(etapaInicial);
                listEtapasObligatorias.Add(etapaFinal);

                return Ok(listEtapasObligatorias);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/EtapaOfertas/5
        [ResponseType(typeof(EtapaOferta))]
        public IHttpActionResult GetEtapaOferta(int id)
        {
            var etapaOferta = db.EtapasOfertas
                .Include(e => e.PuestosEtapaOferta)
                .Include(e => e.PuestosEtapaOferta
                    .Select(pe => pe.Postulaciones
                        .Select(p => p.Profesional)))
                .FirstOrDefault(e => e.Id == id);
            if (etapaOferta == null)
            {
                return NotFound();
            }

            return Ok(etapaOferta);
        }

        // PUT: api/EtapaOfertas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEtapaOferta(int id, EtapaOferta etapaOferta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != etapaOferta.Id)
            {
                return BadRequest();
            }

            db.Entry(etapaOferta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EtapaOfertaExists(id))
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

        // POST: api/EtapaOfertas
        [ResponseType(typeof(EtapaOferta))]
        public IHttpActionResult PostEtapaOferta(EtapaOferta etapaOferta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EtapasOfertas.Add(etapaOferta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = etapaOferta.Id }, etapaOferta);
        }

        // DELETE: api/EtapaOfertas/5
        [ResponseType(typeof(EtapaOferta))]
        public IHttpActionResult DeleteEtapaOferta(int id)
        {
            EtapaOferta etapaOferta = db.EtapasOfertas.Find(id);
            if (etapaOferta == null)
            {
                return NotFound();
            }

            db.EtapasOfertas.Remove(etapaOferta);
            db.SaveChanges();

            return Ok(etapaOferta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EtapaOfertaExists(int id)
        {
            return db.EtapasOfertas.Count(e => e.Id == id) > 0;
        }
    }
}