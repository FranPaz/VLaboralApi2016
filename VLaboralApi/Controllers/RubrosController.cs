using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VLaboralApi.Models;

namespace VLaboralApi.Controllers
{
    public class RubrosController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/TipoContratos
        public IQueryable<Rubro> GetRubroes()
        {
            return db.Rubroes;
        }

        // GET: api/Rubros/5
        [ResponseType(typeof(Rubro))]
        public IHttpActionResult GetRubro(int id)
        {
            try
            {
                var rubro = db.Rubroes
                                    .Include(r => r.Subrubros)
                                    .First(r => r.Id == id);
                if (rubro == null)
                {
                    return NotFound();
                }

                return Ok(rubro);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
               
        }

        // PUT: api/Rubros/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRubro(int id, Rubro rubro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rubro.Id)
            {
                return BadRequest();
            }

            db.Entry(rubro).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RubroExists(id))
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

        // POST: api/Rubros
        [ResponseType(typeof(Rubro))]
        public IHttpActionResult PostRubro(Rubro rubro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            if (rubro == null) return BadRequest();

            db.Rubroes.Add(rubro);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rubro.Id }, rubro);
        }

        // DELETE: api/Rubros/5
        [ResponseType(typeof(Rubro))]
        public IHttpActionResult DeleteRubro(int id)
        {
            var rubro = db.Rubroes.Find(id);
            if (rubro == null)
            {
                return NotFound();
            }

            db.Rubroes.Remove(rubro);
            db.SaveChanges();

            return Ok(rubro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RubroExists(int id)
        {
            return db.Rubroes.Count(e => e.Id == id) > 0;
        }
    }
}