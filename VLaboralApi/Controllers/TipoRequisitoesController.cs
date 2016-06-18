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
    public class TipoRequisitoesController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/TipoRequisitoes
        public IHttpActionResult GetTipoRequisitoes()
        {
            try
            {
                var listTipoRequisito = (from a in db.TipoRequisitoes                                    
                                    select a)
                                    .ToList();

                if (listTipoRequisito == null)
                {
                    return BadRequest("No existen tipos de requisitos");
                }
                else
                {       

                    return Ok(listTipoRequisito);
                }


            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // GET: api/TipoRequisitoes/5
        [ResponseType(typeof(TipoRequisito))]
        public IHttpActionResult GetTipoRequisito(int id)
        {
            TipoRequisito tipoRequisito = db.TipoRequisitoes.Find(id);
            if (tipoRequisito == null)
            {
                return NotFound();
            }

            return Ok(tipoRequisito);
        }

        // PUT: api/TipoRequisitoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoRequisito(int id, TipoRequisito tipoRequisito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoRequisito.Id)
            {
                return BadRequest();
            }

            db.Entry(tipoRequisito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoRequisitoExists(id))
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

        // POST: api/TipoRequisitoes
        [ResponseType(typeof(TipoRequisito))]
        public IHttpActionResult PostTipoRequisito(TipoRequisito tipoRequisito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoRequisitoes.Add(tipoRequisito);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoRequisito.Id }, tipoRequisito);
        }

        // DELETE: api/TipoRequisitoes/5
        [ResponseType(typeof(TipoRequisito))]
        public IHttpActionResult DeleteTipoRequisito(int id)
        {
            TipoRequisito tipoRequisito = db.TipoRequisitoes.Find(id);
            if (tipoRequisito == null)
            {
                return NotFound();
            }

            db.TipoRequisitoes.Remove(tipoRequisito);
            db.SaveChanges();

            return Ok(tipoRequisito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoRequisitoExists(int id)
        {
            return db.TipoRequisitoes.Count(e => e.Id == id) > 0;
        }
    }
}