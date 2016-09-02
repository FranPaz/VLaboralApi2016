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
using System.Web.UI.WebControls;
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var mensaje = "";
                if (EstaPostulado(postulacion, ref mensaje)) return BadRequest(mensaje);

                if (!CumpleRequisitos(postulacion, ref mensaje)) return BadRequest(mensaje);

                //Sluna: obtengo el puestoEtapaOferta correspondiente al Puesto al que desea postularse
                var puestoEtapaOferta = db.PuestoEtapaOfertas
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

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
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

        private bool EstaPostulado(NuevaPostulacion postulacion, ref string mensaje)
        {
            //Sluna: valido si el profesional está postulado en el puesto que pretende postularse

            if (!db.Postulacions
                .Any(p => p.ProfesionalId == postulacion.ProfesionalId
                && p.PuestoEtapaOferta.PuestoId == postulacion.PuestoId)) return false;

            mensaje = "El profesional ya se ha postulado al puesto especificado.";
            return true;
        }

        private bool CumpleRequisitos(NuevaPostulacion postulacion, ref string mensaje)
        {
            var requisitos = db.Requisitos
                                    .Where(r => r.PuestoId == postulacion.PuestoId && r.AutoVerificar && r.Excluyente)
                                    //Sluna: traigo solo los requisitos seleccionados para la atuoverificación y marcados como excluyentes
                                    .Include(r => r.ValoresRequisito)
                                    .Include(r => r.TipoRequisito);

            var profesional = db.Profesionals.FirstOrDefault(p => p.Id == postulacion.ProfesionalId);
            if (profesional == null)
            {
                mensaje =
                    "Ocurrió un error al intentar verificar si el profesional cumple con los requisitos del puesto.";
                return false;
            }
            foreach (var requisito in requisitos)
            {
                    switch (requisito.TipoRequisito.Nombre)
                    {
                        case "Edad":
                            if (profesional.FechaNac != null)
                            {
                                var edad = DateTime.Today.AddTicks(-profesional.FechaNac.Value.Ticks).Year - 1;
                                foreach (var valor in requisito.ValoresRequisito)
                                {
                                    if (edad < valor.Desde)
                                    {
                                        mensaje =
                                            "El postulante no cumple con el requisito de edad necesario para el puesto.";
                                        return false;
                                    }
                                    if (edad > valor.Hasta)
                                    {
                                        mensaje =
                                            "El postulante no cumple con el requisito de edad necesario para el puesto.";
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                mensaje =
                                      "Ocurrió un error al intentar verificar si el profesional cumple con los requisitos del puesto.";
                                return false;
                            }
                            break;
                        case "Sexo":
                            if (profesional.Sexo != null)
                            {
                                if (requisito.ValoresRequisito.Any(valor => profesional.Sexo != valor.Valor))
                                {
                                    mensaje =
                                        "El postulante no cumple con el requisito de sexo necesario para el puesto.";
                                    return false;
                                }
                            }
                            else
                            {
                                mensaje =
                                      "Ocurrió un error al intentar verificar si el profesional cumple con los requisitos del puesto.";
                                return false;
                            }
                            break;
                        case "Identidad":
                            {
                                //SLuna: esto queda muy fiero
                                if (!profesional.IdentidadVerificada)
                                {
                                    mensaje =
                                        "El postulante no cumple con el requisito de tener la identidad verificada necesaria para el puesto.";
                                    return false;
                                }
                            }
                            break;
                        case "Lugar de Residencia":
                            //SLuna: no tenemos nada defino para Lugar de Residencia todavia
                            break;
                        case "Idioma":
                            //SLuna: no tenemos nada defino para Idioma todavia
                            break;
                    }
                }
                
            return true;


        }
    }
}