using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using VLaboralApi.Models;
using VLaboralApi.ClasesAuxiliares;
using VLaboralApi.Services;

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


        // GET: api/Postulaciones
        [ResponseType(typeof(CustomPaginateResult<Postulacion>))]
        public IHttpActionResult GetPostulaciones(int page, int rows)
        {
            try
            {
                var idProfesional = Utiles.GetProfesionalId(User.Identity.GetUserId());
                var data = Utiles.Paginate(new PaginateQueryParameters(page, rows)
                    , db.Postulacions
                           .Where(p => p.ProfesionalId == idProfesional)
                    .Include(p => p.PuestoEtapaOferta.EtapaOferta)
                    .Include(p => p.PuestoEtapaOferta.Puesto)
                    .Include(p => p.PuestoEtapaOferta.EtapaOferta.Oferta)
                    , order => order.OrderBy(c => c.Id));
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



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

                var notificacionHelper = new NotificacionesHelper();
                var notificacion = notificacionHelper.GenerarNotificacionPostulacion(p.Id);

                return Ok(notificacion);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT: api/Postulaciones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPostulacion(Postulacion postulacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _postulacion = db.Postulacions.FirstOrDefault(p => p.Id == postulacion.Id);


            //sluna: Primero valido que la etapaOferta esté abierta, es decir, FechaFin == null
            var etapaOferta =
                db.EtapasOfertas.FirstOrDefault(
                    eo => eo.PuestosEtapaOferta.Any(peo => peo.Id == postulacion.PuestoEtapaOfertaId));

            if (etapaOferta.FechaFin != null) { return BadRequest("La postulación que desea modificar pertence a una etapa que ya está cerrada y por lo tanto no puede ser modificada."); }

            if (_postulacion == null) return BadRequest("No se ha encontrado la postulación");
            //sluna: Actualizo los atributos de las postulaciones.
            _postulacion.Valoracion = postulacion.Valoracion;
            _postulacion.Comentario = postulacion.Comentario;
            _postulacion.PasaEtapa = postulacion.PasaEtapa;
            db.SaveChanges();
            return Ok(db.Postulacions.Find(_postulacion.Id));
            //var postulaciones = db.Postulacions.Where(p => p.PuestoEtapaOfertaId == resultadoPostulacion.PuestoEstapaOfertaId);
            //foreach (var postulacion in resultadoPostulacion.Postulaciones)
            //{
            //    var postulacionBd =
            //        postulaciones.FirstOrDefault(p => p.Id == postulacion.Id);

            //    if (postulacion == null) continue;//sluna: if invertido para reducir anidamiento

            //    postulacion.Valoracion = postulacionBd.Valoracion;
            //    postulacion.Comentario = postulacionBd.Comentario;
            //    postulacion.PasaEtapa = postulacionBd.PasaEtapa;
            //}


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