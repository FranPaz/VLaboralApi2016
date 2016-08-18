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
    public class PostulacionesController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();


        //// POST: api/Postulaciones/AptoPostulacion
        //[HttpPost]
        //[Route("AptoPostulacion")]
        //[ResponseType(typeof(Postulacion))]
        //public IHttpActionResult AptoPostulacion(NuevaPostulacion postulacion)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var mensaje = "";
        //    if (YaEstaPostulado(postulacion, ref mensaje)) return BadRequest(mensaje);

        //    return Ok();
        //}

        // POST: api/Postulaciones
        [ResponseType(typeof(Postulacion))]
        public IHttpActionResult PostPostulacion(NuevaPostulacion postulacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mensaje = "";
            if (YaEstaPostulado(postulacion, ref mensaje)) return BadRequest(mensaje);

            //sluna: TODO: hay que agregar la validacion de requisitos aquí.

            //Sluna: obtengo el puestoEtapaOferta correspondiente al Puesto al que desea postularse
            var puestoEtapaOferta=db.PuestoEtapaOfertas
                .FirstOrDefault(peo => peo.PuestoId == postulacion.PuestoId
                                && peo.EtapaOferta.TipoEtapa.EsInicial == true); //me parece mejor esto
                                //   && peo.EtapaOferta.IdEtapaAnterior == 0); //que sea la etapa inicial
            // && peo.EtapaOfertaId.Equals(peo.Puesto.Oferta.IdEtapaActual) //que sea la etapa actual 

            if (puestoEtapaOferta == null)
            {
                return BadRequest();
            }

            var p = new Postulacion
            {
                ProfesionalId = postulacion.ProfesionalId,
                Fecha = DateTime.Now,
                PuestoEtapaOfertaId = puestoEtapaOferta.Id
            };

            db.Postulacions.Add(p);
            db.SaveChanges();

            return Ok();
            //  return CreatedAtRoute("DefaultApi", new { id = postulacion.Id }, postulacion);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostulacionExists(int id)
        {
            return db.Postulacions.Count(e => e.Id == id) > 0;
        }

        private bool YaEstaPostulado(NuevaPostulacion postulacion, ref string mensaje)
        {
            //Sluna: valido si el profesional está postulado en el puesto que pretende postularse

            if (!db.Postulacions
                .Any(p => p.ProfesionalId == postulacion.ProfesionalId
                && p.PuestoEtapaOferta.PuestoId == postulacion.PuestoId)) return false;

            mensaje = "El profesional ya se ha postulado al puesto especificado.";
            return true;
        }
    }
}