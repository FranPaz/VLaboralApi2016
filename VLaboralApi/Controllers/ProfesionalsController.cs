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
            try
            {
                var profesional = db.Profesionals
                    .Where(p => p.Id == id)
                    .Include(s => s.Subrubros.Select(r => r.Rubro))
                    .Include(i => i.IdentificacionesProfesional.Select(ti => ti.TipoIdentificacionProfesional))
                    .Include(exp => exp.ExperienciasLaborales.Select(e => e.Empresa))                    
                    .Include(cur => cur.Cursos)
                    .Include(educ => educ.Educaciones)
                    .Include(educ => educ.Educaciones.Select(nivel => nivel.TipoNivelEstudio))
                    .Include(idioma => idioma.IdiomasConocidos.Select(idio => idio.Idioma))
                    .Include(idioma => idioma.IdiomasConocidos.Select(comp => comp.CompetenciaIdioma))
                    .FirstOrDefault();

                if (profesional == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(profesional);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Profesionals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProfesional(int id, Profesional profesional)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != profesional.Id)
                {
                    return BadRequest();
                }

                //fpaz: obtengo la entidad profesional original guardada en la bd
                var profDb = db.Profesionals
                    .Where(p => p.Id == id)
                    .Include(s => s.Subrubros)  
                    .Include(i => i.IdentificacionesProfesional)
                    .FirstOrDefault();

                
                //fpaz: actualizo las propiedades escalares del objeto profesional que recibo como parametro
                db.Entry(profDb).CurrentValues.SetValues(profesional);

                #region fpaz: update de subrubros
                //fpaz: elimino los subrubros que ya no se hayan enviado en el array de subrubros modificados
                foreach (var dbSubRubro in profDb.Subrubros.ToList())
                {
                    //fpaz: para cada subrubro asociado actualmente al profesional en la bd
                    if (!profesional.Subrubros.Any(s => s.Id == dbSubRubro.Id)) //busco si en el array de subrubros enviados como parametros, alguno de los objetos subrubros coincide con el de la base de datos
                    {
                        //si no encuentro al subrubro de la bd en el array ingresado como parametro, elimino la relacion entre ese subrubro y el profesional
                        profDb.Subrubros.Remove(dbSubRubro);
                    }
                }

                //fpaz: agrego los nuevos subrubros enviados en el array de subrubros del profesional
                foreach (var prmSubRubro in profesional.Subrubros)
                {
                    //fpaz: para cada subrubro ingresado como parametro
                    if (!profDb.Subrubros.Any(s => s.Id == prmSubRubro.Id)) //busco si el subrubro ingresado como parametro en el cliente esta actualmente en el array de subrubros asociados al profesional
                    {
                        //si el subrubro no esta relacionado
                        var a = db.SubRubros.Find(prmSubRubro.Id); //obtengo el objeto subrubro (esto por que es una relacion M a M)
                        profDb.Subrubros.Add(a); //agrego el subrubro al array de subrubros del profesional
                    }
                }
                #endregion

                #region fpaz: update identificaciones del profesional
                //fpaz: elimino las identificaciones del profesional que no se hayan enviado en el array de identificaciones modificadas
                foreach (var dbIdent in profDb.IdentificacionesProfesional.ToList())
                {
                    //fpaz: para cada identificacion asociado actualmente al profesional en la bd
                    if (!profesional.IdentificacionesProfesional.Any(s => s.Id == dbIdent.Id)) //busco si en el array de identificaciones enviados como parametros, alguno de los objetos identificacion coincide con el de la base de datos
                    {
                        //si no encuentro la identificacion en el array ingresado como parametro, elimino la relacion entre esa identificacion y el profesional
                        db.IdentificacionesProfesional.Remove(dbIdent);
                    }
                }

                //fpaz: agrego o actualizo las identificaciones del profesional
                foreach (var prmIdent in profesional.IdentificacionesProfesional)
                {
                    var dbIdent = profDb.IdentificacionesProfesional.FirstOrDefault(s => s.Id == prmIdent.Id); //busco si la identificacion ingresada como parametro esta asociada al profesional en la bd
                    if (dbIdent != null && dbIdent.Id > 0)
                        // Update de la identificacion
                        db.Entry(dbIdent).CurrentValues.SetValues(prmIdent);
                    else
                        //agrego una nueva identificacion al profesional
                        profDb.IdentificacionesProfesional.Add(prmIdent);
                }

                #endregion


                db.SaveChanges();

                return Ok(profDb);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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