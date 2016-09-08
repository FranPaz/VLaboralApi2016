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
    public class ExperienciaLaboralsController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/ExperienciaLaborals
        public IQueryable<ExperienciaLaboral> GetExperienciaLaborals()
        {
            return db.ExperienciaLaborals;
        }

        // GET: api/ExperienciaLaborals/5
        [ResponseType(typeof(ExperienciaLaboral))]
        public IHttpActionResult GetExperienciaLaboral(int id)
        {
            ExperienciaLaboral experienciaLaboral = db.ExperienciaLaborals.Find(id);
            if (experienciaLaboral == null)
            {
                return NotFound();
            }

            return Ok(experienciaLaboral);
        }

        // PUT: api/ExperienciaLaborals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutExperienciaLaboral(int id, ExperienciaLaboral experienciaLaboral)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != experienciaLaboral.Id)
            {
                return BadRequest();
            }

            db.Entry(experienciaLaboral).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperienciaLaboralExists(id))
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

        // POST: api/ExperienciaLaborals
        [ResponseType(typeof(ExperienciaLaboral))]
        public IHttpActionResult PostExperienciaLaboral(ExperienciaLaboral experienciaLaboral)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExperienciaLaborals.Add(experienciaLaboral);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = experienciaLaboral.Id }, experienciaLaboral);
        }

        // DELETE: api/ExperienciaLaborals/5
        [ResponseType(typeof(ExperienciaLaboral))]
        public IHttpActionResult DeleteExperienciaLaboral(int id)
        {
            ExperienciaLaboral experienciaLaboral = db.ExperienciaLaborals.Find(id);
            if (experienciaLaboral == null)
            {
                return NotFound();
            }

            db.ExperienciaLaborals.Remove(experienciaLaboral);
            db.SaveChanges();

            return Ok(experienciaLaboral);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExperienciaLaboralExists(int id)
        {
            return db.ExperienciaLaborals.Count(e => e.Id == id) > 0;
        }
    }
}