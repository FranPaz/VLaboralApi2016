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
    public class OfertasController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Ofertas
        public IQueryable<Oferta> GetOfertas()
        {
            return db.Ofertas;
        }

        // GET: api/Ofertas/5
        [ResponseType(typeof(Oferta))]
        public IHttpActionResult GetOferta(int id)
        {
            Oferta oferta = db.Ofertas.Find(id);
            if (oferta == null)
            {
                return NotFound();
            }

            return Ok(oferta);
        }

        // GET: api/Ofertas/5        
        public IHttpActionResult GetOfertas(int prmIdProfesional) //fpaz: funcion para obtener las ofertas relacionadas a los subrubros del empleado
        {
            try
            {
                //fpaz: obtengo los datos del profesional incluyendo los subrubros
                var prof = (from p in db.Profesionals
                            where p.Id == prmIdProfesional
                            select p)
                            .Include(s=>s.Subrubros)
                            .FirstOrDefault();

                //fpaz: armo un array solo con los Ids de los subrubros, sirve para el where en las ofertas
                var subs = (from s in prof.Subrubros
                            select s.Id).ToList();

                var listOfertas = new List<Oferta>();
                if (subs.Count > 0)
                {
                    //fpaz: obtengo el listado de ofertas que tengan al menos un puesto con algun subrubro cargado por el empleado
                    listOfertas = (from p in db.Puestos
                                       join o in db.Ofertas
                                       on p.OfertaId equals o.Id
                                       where
                                           //DateTime.Parse(o.FechaFinConvocatoria).CompareTo(DateTime.Now) > 0  && 
                                       p.Subrubros.Any(s => subs.Contains(s.Id)) // consulto si existe algunos de los subrubros de los puestos que este contenido dentro del array de Ids de Subrubros del Empleado
                                       select o)
                             .Take(10) //fpaz: cantidad de ofertas a devolver
                             .ToList();
                }
                else
                {
                    //fpaz: si el empleado no tiene cargado ningun subrubro, se devuelve las ultimas x ofertas
                    listOfertas = db.Ofertas
                            .Take(1) //fpaz: cantidad de ofertas a devolver
                            .ToList();
                }
                return Ok(listOfertas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        // PUT: api/Ofertas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOferta(int id, Oferta oferta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != oferta.Id)
            {
                return BadRequest();
            }

            db.Entry(oferta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfertaExists(id))
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

        // POST: api/Ofertas
        [ResponseType(typeof(Oferta))]
        public IHttpActionResult PostOferta(Oferta oferta)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Ofertas.Add(oferta);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Ofertas/5
        [ResponseType(typeof(Oferta))]
        public IHttpActionResult DeleteOferta(int id)
        {
            Oferta oferta = db.Ofertas.Find(id);
            if (oferta == null)
            {
                return NotFound();
            }

            db.Ofertas.Remove(oferta);
            db.SaveChanges();

            return Ok(oferta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OfertaExists(int id)
        {
            return db.Ofertas.Count(e => e.Id == id) > 0;
        }
    }
}