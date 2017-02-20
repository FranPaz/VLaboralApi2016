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
    public class CompetenciaIdiomasController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/CompetenciaIdiomas
        public IHttpActionResult GetCompetenciaIdiomas()
        {
            try
            {
                var listCompetencias = db.CompetenciaIdiomas.ToList();
                if (listCompetencias == null)
                {
                    return BadRequest("No se cargaron competencias de idiomas");

                }
                else
                {
                    return Ok(listCompetencias);
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: api/CompetenciaIdiomas/5
        [ResponseType(typeof(CompetenciaIdioma))]
        public IHttpActionResult GetCompetenciaIdioma(int id)
        {
            CompetenciaIdioma competenciaIdioma = db.CompetenciaIdiomas.Find(id);
            if (competenciaIdioma == null)
            {
                return NotFound();
            }

            return Ok(competenciaIdioma);
        }

        // PUT: api/CompetenciaIdiomas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCompetenciaIdioma(int id, CompetenciaIdioma competenciaIdioma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != competenciaIdioma.Id)
            {
                return BadRequest();
            }

            db.Entry(competenciaIdioma).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetenciaIdiomaExists(id))
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

        // POST: api/CompetenciaIdiomas
        [ResponseType(typeof(CompetenciaIdioma))]
        public IHttpActionResult PostCompetenciaIdioma(CompetenciaIdioma competenciaIdioma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CompetenciaIdiomas.Add(competenciaIdioma);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = competenciaIdioma.Id }, competenciaIdioma);
        }

        // DELETE: api/CompetenciaIdiomas/5
        [ResponseType(typeof(CompetenciaIdioma))]
        public IHttpActionResult DeleteCompetenciaIdioma(int id)
        {
            CompetenciaIdioma competenciaIdioma = db.CompetenciaIdiomas.Find(id);
            if (competenciaIdioma == null)
            {
                return NotFound();
            }

            db.CompetenciaIdiomas.Remove(competenciaIdioma);
            db.SaveChanges();

            return Ok(competenciaIdioma);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompetenciaIdiomaExists(int id)
        {
            return db.CompetenciaIdiomas.Count(e => e.Id == id) > 0;
        }
    }
}