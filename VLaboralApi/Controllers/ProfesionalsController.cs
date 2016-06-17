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
    public class ProfesionalsController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Profesionals
        public IQueryable<Profesional> GetProfesionals()
        {
            return db.Profesionals;
        }

        // GET: api/Profesionals/5
        [ResponseType(typeof(Profesional))]
        public IHttpActionResult GetProfesional(int id)
        {
            Profesional profesional = db.Profesionals.Find(id);
            if (profesional == null)
            {
                return NotFound();
            }

            return Ok(profesional);
        }

        // PUT: api/Profesionals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProfesional(int id, Profesional profesional)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profesional.Id)
            {
                return BadRequest();
            }

            db.Entry(profesional).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfesionalExists(id))
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

        // POST: api/Profesionals
        [ResponseType(typeof(Profesional))]
        public IHttpActionResult PostProfesional(Profesional profesional)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Profesionals.Add(profesional);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = profesional.Id }, profesional);
        }

        // DELETE: api/Profesionals/5
        [ResponseType(typeof(Profesional))]
        public IHttpActionResult DeleteProfesional(int id)
        {
            Profesional profesional = db.Profesionals.Find(id);
            if (profesional == null)
            {
                return NotFound();
            }

            db.Profesionals.Remove(profesional);
            db.SaveChanges();

            return Ok(profesional);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfesionalExists(int id)
        {
            return db.Profesionals.Count(e => e.Id == id) > 0;
        }
    }
}