using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VLaboralApi.Models;

namespace VLaboralApi.Controllers
{
    public class VerificacionExperienciaLaboralsController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/VerificacionExperienciaLaborals
        public IQueryable<VerificacionExperienciaLaboral> GetVerificacionExperienciaLaborals()
        {
            return db.VerificacionExperienciaLaborals;
        }

        // GET: api/VerificacionExperienciaLaborals/5
        [ResponseType(typeof(VerificacionExperienciaLaboral))]
        public IHttpActionResult GetVerificacionExperienciaLaboral(int id)
        {
            VerificacionExperienciaLaboral verificacionExperienciaLaboral = db.VerificacionExperienciaLaborals.Find(id);
            if (verificacionExperienciaLaboral == null)
            {
                return NotFound();
            }

            return Ok(verificacionExperienciaLaboral);
        }

        // PUT: api/VerificacionExperienciaLaborals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVerificacionExperienciaLaboral(int id, VerificacionExperienciaLaboral verificacionExperienciaLaboral)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != verificacionExperienciaLaboral.Id)
            {
                return BadRequest();
            }

            db.Entry(verificacionExperienciaLaboral).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VerificacionExperienciaLaboralExists(id))
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

        // POST: api/VerificacionExperienciaLaborals
        [ResponseType(typeof(VerificacionExperienciaLaboral))]
        public IHttpActionResult PostVerificacionExperienciaLaboral(VerificacionExperienciaLaboral verificacionExperienciaLaboral)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VerificacionExperienciaLaborals.Add(verificacionExperienciaLaboral);

            try
            {

                db.SaveChanges();

                //iafar: busco la experiencia a la que estoy verificando y la marco como verificada
               ExperienciaLaboral experienciaLaboralMod = (from exp in db.ExperienciaLaborals
                                         where exp.Id == verificacionExperienciaLaboral.Id
                                         select exp)
                                        .FirstOrDefault();
               experienciaLaboralMod.isVerificada = true;

                //iafar: promedio la valoracion del profesional
               


               db.Entry(experienciaLaboralMod).State = EntityState.Modified;
               db.SaveChanges();

            }
            catch (DbUpdateException)
            {
                if (VerificacionExperienciaLaboralExists(verificacionExperienciaLaboral.Id))
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok();
        }

        // DELETE: api/VerificacionExperienciaLaborals/5
        [ResponseType(typeof(VerificacionExperienciaLaboral))]
        public IHttpActionResult DeleteVerificacionExperienciaLaboral(int id)
        {
            VerificacionExperienciaLaboral verificacionExperienciaLaboral = db.VerificacionExperienciaLaborals.Find(id);
            if (verificacionExperienciaLaboral == null)
            {
                return NotFound();
            }

            db.VerificacionExperienciaLaborals.Remove(verificacionExperienciaLaboral);
            db.SaveChanges();

            return Ok(verificacionExperienciaLaboral);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VerificacionExperienciaLaboralExists(int id)
        {
            return db.VerificacionExperienciaLaborals.Count(e => e.Id == id) > 0;
        }
    }
}