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
    public class CursosController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Cursos
        public IQueryable<Curso_Certificacion> GetEstudios()
        {
            return db.Cursos;
        }

        // GET: api/Cursos/5
        [ResponseType(typeof(Curso_Certificacion))]
        public IHttpActionResult GetCurso_Certificacion(int id)
        {
            Curso_Certificacion curso_Certificacion = db.Cursos.Find(id);
            if (curso_Certificacion == null)
            {
                return NotFound();
            }

            return Ok(curso_Certificacion);
        }

        // PUT: api/Cursos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCurso_Certificacion(int id, Curso_Certificacion curso_Certificacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != curso_Certificacion.Id)
            {
                return BadRequest();
            }

            db.Entry(curso_Certificacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Curso_CertificacionExists(id))
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

        // POST: api/Cursos
        [ResponseType(typeof(Curso_Certificacion))]
        public IHttpActionResult PostCurso_Certificacion(Curso_Certificacion curso_Certificacion) //fpaz: alta de nuevo curso realizado por el profesional
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Estudios.Add(curso_Certificacion);
                db.SaveChanges();
                return Ok(curso_Certificacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        // DELETE: api/Cursos/5
        [ResponseType(typeof(Curso_Certificacion))]
        public IHttpActionResult DeleteCurso_Certificacion(int id)
        {
            Curso_Certificacion curso_Certificacion = db.Cursos.Find(id);
            if (curso_Certificacion == null)
            {
                return NotFound();
            }

            db.Estudios.Remove(curso_Certificacion);
            db.SaveChanges();

            return Ok(curso_Certificacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Curso_CertificacionExists(int id)
        {
            return db.Estudios.Count(e => e.Id == id) > 0;
        }
    }
}