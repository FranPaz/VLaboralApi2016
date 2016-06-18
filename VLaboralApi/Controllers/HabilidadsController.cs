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
    public class HabilidadsController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Habilidads
        public IHttpActionResult GetHabilidads()
        {
            try
            {
                var listHabilidades = (from a in db.Habilidads
                                         select a)
                                    .ToList();

                if (listHabilidades == null)
                {
                    return BadRequest("No existen Habilidades");
                }
                else
                {

                    return Ok(listHabilidades);
                }


            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // GET: api/Habilidads/5
        [ResponseType(typeof(Habilidad))]
        public IHttpActionResult GetHabilidad(int id)
        {
            Habilidad habilidad = db.Habilidads.Find(id);
            if (habilidad == null)
            {
                return NotFound();
            }

            return Ok(habilidad);
        }

        // PUT: api/Habilidads/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHabilidad(int id, Habilidad habilidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != habilidad.Id)
            {
                return BadRequest();
            }

            db.Entry(habilidad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabilidadExists(id))
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

        // POST: api/Habilidads
        [ResponseType(typeof(Habilidad))]
        public IHttpActionResult PostHabilidad(Habilidad habilidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Habilidads.Add(habilidad);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = habilidad.Id }, habilidad);
        }

        // DELETE: api/Habilidads/5
        [ResponseType(typeof(Habilidad))]
        public IHttpActionResult DeleteHabilidad(int id)
        {
            Habilidad habilidad = db.Habilidads.Find(id);
            if (habilidad == null)
            {
                return NotFound();
            }

            db.Habilidads.Remove(habilidad);
            db.SaveChanges();

            return Ok(habilidad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HabilidadExists(int id)
        {
            return db.Habilidads.Count(e => e.Id == id) > 0;
        }
    }
}