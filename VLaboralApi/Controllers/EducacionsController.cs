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
    public class EducacionsController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Educacions
        public IQueryable<Educacion> GetEstudios()
        {
            return db.Educacions;
        }

        // GET: api/Educacions/5
        [ResponseType(typeof(Educacion))]
        public IHttpActionResult GetEducacion(int id)
        {
            Educacion educacion = db.Educacions.Find(id);
            if (educacion == null)
            {
                return NotFound();
            }

            return Ok(educacion);
        }

        // PUT: api/Educacions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEducacion(int id, Educacion educacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != educacion.Id)
            {
                return BadRequest();
            }

            db.Entry(educacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EducacionExists(id))
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

        // POST: api/Educacions
        [ResponseType(typeof(Educacion))]
        public IHttpActionResult PostEducacion(Educacion educacion) //fpaz: funcion para guardar una nueva educacion en la bd
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Estudios.Add(educacion);
                db.SaveChanges();

                educacion.TipoNivelEstudio = db.TipoNivelEstudios.Find(educacion.TipoNivelEstudioId);

                return Ok(educacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        // DELETE: api/Educacions/5
        [ResponseType(typeof(Educacion))]
        public IHttpActionResult DeleteEducacion(int id)
        {
            Educacion educacion = db.Educacions.Find(id);
            if (educacion == null)
            {
                return NotFound();
            }

            db.Estudios.Remove(educacion);
            db.SaveChanges();

            return Ok(educacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EducacionExists(int id)
        {
            return db.Estudios.Count(e => e.Id == id) > 0;
        }
    }
}