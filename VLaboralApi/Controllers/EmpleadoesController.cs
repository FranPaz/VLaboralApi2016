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
using Microsoft.AspNet.Identity;
using VLaboralApi.ClasesAuxiliares;
using VLaboralApi.Models;
using VLaboralApi.ViewModels.Empleados;

namespace VLaboralApi.Controllers
{
    public class EmpleadoesController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Empleadoes
        public IQueryable<Empleado> GetEmpleadoes()
        {
            return db.Empleadoes;
        }

        
        [ResponseType(typeof(Empleado))]
        public IHttpActionResult GetEmpleado([FromUri] IdentificacionProfesional identificacion)
        {

            var empresaId = Utiles.GetEmpresaId(User.Identity.GetUserId());
            Empleado empleado = null;
            if (empresaId != null)
            {
                empleado = db.Empleadoes
                    .FirstOrDefault(e => e.EmpresaId == empresaId &
                                         e.IdentificacionesEmpleado
                                             .Any(ie => ie.TipoIdentificacionEmpleadoId == identificacion.TipoIdentificacionProfesionalId & ie.Valor == identificacion.Valor));
            }
            return Ok(empleado);
        }

        // GET: api/Empleadoes/5
        [ResponseType(typeof(Empleado))]
        public IHttpActionResult GetEmpleado(int id)
        {
            Empleado empleado = db.Empleadoes.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }

            return Ok(empleado);
        }

        // PUT: api/Empleadoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmpleado(int id, Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empleado.Id)
            {
                return BadRequest();
            }

            db.Entry(empleado).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(id))
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

        // POST: api/Empleadoes
        [ResponseType(typeof(Empleado))]
        public IHttpActionResult PostEmpleado(EmpleadoVM empleadoVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (db.Empresas.Find(empleadoVm.EmpresaId) == null) return BadRequest("Verificar EmpresaId");

            var profesional = new Profesional();
            var empleado = new Empleado();

            if (empleadoVm.ProfesionalId == null)
            {
                GuardarProfesional(empleadoVm, profesional);
                empleado = GuardarEmpleado(empleadoVm, profesional);
            }
            else
            {
                profesional = db.Profesionals.Find(empleadoVm.ProfesionalId);

                if (profesional == null) return BadRequest("Verificar ProfesionalId");
                
                empleado = GuardarEmpleado(empleadoVm, profesional);
                CargarExperienciasLaborales(empleadoVm, profesional);
                db.SaveChanges();
            }
            return Ok(empleado);
           
        }

        private void GuardarProfesional(EmpleadoVM empleadoVm, Profesional profesional)
        {
            profesional.Apellido = empleadoVm.Apellido;
            profesional.Nombre = empleadoVm.Nombre;
            profesional.FechaNac = empleadoVm.FechaNac;
            profesional.Nacionalidad = empleadoVm.Nacionalidad;
            profesional.Domicilio = empleadoVm.Domicilio;
            profesional.Sexo = empleadoVm.Sexo.ToString();
            CargarExperienciasLaborales(empleadoVm, profesional);
            db.Profesionals.Add(profesional);
            db.SaveChanges();
        }

        private Empleado GuardarEmpleado(EmpleadoVM empleadoVm, Profesional profesional)
        {
            var empleado = new Empleado
            {
                Apellido = empleadoVm.Apellido,
                Nombre = empleadoVm.Nombre,
                FechaNac = empleadoVm.FechaNac,
                Nacionalidad = empleadoVm.Nacionalidad,
                Domicilio = empleadoVm.Domicilio,
                Sexo = empleadoVm.Sexo,
                ProfesionalId = profesional.Id,
                EmpresaId =  empleadoVm.EmpresaId
            };
            db.Empleadoes.Add(empleado);
            db.SaveChanges();
            return empleado;
        }

        private void CargarExperienciasLaborales(EmpleadoVM empleadoVm, Profesional profesional)
        {
            foreach (var experiencia in empleadoVm.ExperienciasLaborales)
            {
                var experienciaLaboral = new ExperienciaLaboral
                {
                    ProfesionalId = profesional.Id,
                    Descripcion = experiencia.Descripcion,
                    EmpresaId = experiencia.EmpresaId,
                    FechaCreacion = DateTime.Now.Date,
                    PeriodoDesde = experiencia.PeriodoDesde,
                    PeriodoHasta = experiencia.PeriodoHasta,
                    Puesto = experiencia.Puesto,
                    Ubicacion = experiencia.Ubicacion,
                    idUsuarioCreacion = User.Identity.GetUserId()
                };
                profesional.ExperienciasLaborales.Add(experienciaLaboral);
            }
        }

        // DELETE: api/Empleadoes/5
        [ResponseType(typeof(Empleado))]
        public IHttpActionResult DeleteEmpleado(int id)
        {
            Empleado empleado = db.Empleadoes.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }

            db.Empleadoes.Remove(empleado);
            db.SaveChanges();

            return Ok(empleado);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmpleadoExists(int id)
        {
            return db.Empleadoes.Count(e => e.Id == id) > 0;
        }
    }
}