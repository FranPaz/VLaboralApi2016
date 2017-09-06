using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using VLaboralApi.ClasesAuxiliares;
using VLaboralApi.Hubs;
using VLaboralApi.Models;
using VLaboralApi.Providers;
using VLaboralApi.Services;

namespace VLaboralApi.Controllers
{
    public class EmpresasController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Empresas
        public IHttpActionResult GetEmpresas()
        {
            try
            {
                var empresas = db.Empresas.ToList();
                if (empresas == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(empresas);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Empresas/5
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult GetEmpresa(int id)
        {
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return NotFound();
            }

            return Ok(empresa);
        }

        // PUT: api/Empresas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmpresa(int id, Empresa empresa)
        {
            try
            {
                //var tipoUsuario = Utiles.GetTipoUsuario(User.Identity.GetUserId());
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empresa.Id)
                {
                    return BadRequest();
                }
                //if (tipoUsuario == Utiles.TiposUsuario.empresa)
                //{
                    
                //}
                //else {
                //    return BadRequest();
                
                //}
                if (id != empresa.Id)
                {
                    return BadRequest();
                }
                var empresaBd = db.Empresas
                    .Where(e => e.Id == id)
                    .Include(emp => emp.IdentificacionesEmpresa)
                    .FirstOrDefault();
                db.Entry(empresaBd).CurrentValues.SetValues(empresa);

                //foreach (var dbIdent in empresaBd.IdentificacionesEmpresa.ToList())
                //{
                //    if (empresa.IdentificacionesEmpresa.All(i => i.Id != dbIdent.Id))
                //    {
                //        db.IdentificacionesEmpresa.Remove(dbIdent);
                //    }

                //}
                //foreach (var prmIdent in empresa.IdentificacionesEmpresa)
                //{
                //    var dbIdent = empresaBd.IdentificacionesEmpresa.FirstOrDefault(s => s.Id == prmIdent.Id);
                //    if (dbIdent != null && dbIdent.Id > 0)
                //    {
                //        db.Entry(dbIdent).CurrentValues.SetValues(prmIdent);

                //    }
                //    else
                //    {
                //        prmIdent.TipoIdentificacionEmpresaId = prmIdent.TipoIdentificacionEmpresa.Id;
                //        prmIdent.TipoIdentificacionEmpresa = null;
                //        empresaBd.IdentificacionesEmpresa.Add(prmIdent);
                //    }
                //}
                db.SaveChanges();

                return Ok(empresaBd);

                
                
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            
            }
            

            

            //db.Entry(empresa).State = EntityState.Modified;

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpresaExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Empresas
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult PostEmpresa(Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            empresa.Domicilio = null; //sluna: null hasta que definamos bien esto.
            empresa.DomicilioId = null; //sluna: null hasta que definamos bien esto.

            db.Empresas.Add(empresa);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = empresa.Id }, empresa);
        }

        // DELETE: api/Empresas/5
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult DeleteEmpresa(int id)
        {
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return NotFound();
            }

            db.Empresas.Remove(empresa);
            db.SaveChanges();

            return Ok(empresa);
        }

        //kike: alta de imagenes de empresa
        //POST: api/Empresa/Imagenes/5
        [Route("api/Empresa/Imagenes")]
        [ResponseType(typeof(void))]
        public IHttpActionResult postImagenEmpresa(ImagenEmpresa imagenEmpresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.ImagenEmpresa.Add(imagenEmpresa);
            db.SaveChanges();

            return Ok();               
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmpresaExists(int id)
        {
            return db.Empresas.Count(e => e.Id == id) > 0;
        }
    }
}