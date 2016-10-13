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
using VLaboralApi.ClasesAuxiliares;
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

        [Route("api/ExperienciaLaboral/PendientesValidar")] //iafar: trae todas las experiencias que necesiten ser validadas por mi empresa
        public IHttpActionResult GetExperienciaLaboralPendiente(int idEmpresa)
        {
            try
            {
                var listExperienciasPro = (from exp in db.ExperienciaLaborals
                                           where (exp.EmpresaId == idEmpresa)
                                           && (exp.isVerificada == false)
                                           select exp)
                                       .ToList();


                if (listExperienciasPro == null)
                {
                    return NotFound();
                }



                return Ok(listExperienciasPro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }

        }



        [Route("api/ExperienciaLaboral/Verificacion")] //iafar: obtiene el detalle de una experiencia en particular
        public IHttpActionResult GetExperienciaLaboralVerificar(int idExperiencia)
        {
            try 
	        {
                var experienciaPro = (from exp in db.ExperienciaLaborals
                                        where (exp.Id== idExperiencia)
                                        select exp)
                                        .Include(pro => pro.Profesional)
                                       .FirstOrDefault();
               
                
                                     
                if (experienciaPro== null)
                {
                    return NotFound();
                }
                experienciaPro.Profesional.ExperienciasLaborales = null;
                return Ok(experienciaPro);
	        }
	            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }


        // GET: api/ExperienciaLaborals/5
        [ResponseType(typeof(ExperienciaLaboral))]
        public IHttpActionResult GetExperienciaLaboral(int id)
        {
            var experienciaLaboral = (from el in db.ExperienciaLaborals
                                      where el.Id == id
                                      select el)
                                    .Include(elv => elv.VerificacionExperienciaLaboral)
                                    .FirstOrDefault();

            //ExperienciaLaboral experienciaLaboral = db.ExperienciaLaborals.Find(id);
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
                //iafar: generar notificacion de experiencia validada para profesional
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperienciaLaboralExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

          
        }

        // POST: api/ExperienciaLaborals
        [ResponseType(typeof(ExperienciaLaboral))]
        public IHttpActionResult PostExperienciaLaboral(ExperienciaLaboral experienciaLaboral) //fpaz: funcion que guarda una nueva experiencia laboral de un profesional en particular
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.ExperienciaLaborals.Add(experienciaLaboral);
                db.SaveChanges();

                if (experienciaLaboral.EmpresaId != null)
                {
                    experienciaLaboral.Empresa = db.Empresas.Find(experienciaLaboral.EmpresaId);
                    //*TODO: dar de alta la notificacion de nueva experiencia laboral cargada para la validacion por parte de la empresa
                    // a partir del usuario que dio de alta la exp
                    var notificacionHelper = new NotificacionesHelper(); 

                    var notificacion = notificacionHelper.generarNotificacionExperiencia(experienciaLaboral.Id);

                    return Ok(notificacion);
                }
                else
                {
                    return Ok(experienciaLaboral);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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