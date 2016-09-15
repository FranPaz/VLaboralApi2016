using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public abstract class Notificacion
    {
        public int Id { get; set; }
        public string EmisorId { get; set; }
        public string ReceptorId { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaLectura { get; set; }

        public DateTime FechaPublicacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }

        //sluna: 1 a m con TipoNotificacion (uno)
        public int TipoNotificacionId { get; set; }
        public virtual TipoNotificacion TipoNotificacion { get; set; }
    }

    public class NotificacionExperiencia : Notificacion
    {
        public int? ExperienciaId { get; set; }
        public virtual ExperienciaLaboral ExperienciaLaboral { get; set; }
    }

    public class NotificacionPostulacion : Notificacion
    {
        public int? PostulacionId { get; set; }
        public virtual Postulacion Postulacion { get; set; }

        public int? EtapaOfertaId { get; set; }
        public virtual EtapaOferta EtapaOferta { get; set; }
    }

    public class TipoNotificacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string TipoEmisorId { get; set; }
        public string TipoReceptorId { get; set; }

        //sluna: relacion 1 a M con Notificacion (muchos)
        public virtual ICollection<Notificacion> Notificaciones { get; set; }
    }
}