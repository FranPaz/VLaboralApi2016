using System;

namespace VLaboralApi.Models
{
    public abstract class Notificacion
    {
        public int Id { get; set; }

        public int? EmisorId { get; set; }
        //public virtual IdentityUser Emisor { get; set; }

        public int? ReceptorId { get; set; }
        //public virtual IdentityUser Receptor { get; set; }

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
    }

    public class NotificacionInvitacionOferta : Notificacion
    {
        public int? OfertaId { get; set; }
        public virtual Oferta Oferta { get; set; }
    }

    //public class NotificacionNovedadesPostulacion : Notificacion
    //{
    //    public int? EtapaOfertaId { get; set; }
    //    public virtual EtapaOferta EtapaOferta { get; set; }
    //}

    public class TipoNotificacion
    {
        public int Id { get; set; }
        public string Valor { get; set; } // NP: Notificacion Postulacion, NE: NotificacionExperiencia, etc
        public string Descripcion { get; set; }

        public string Titulo { get; set; }
        public string Mensaje { get; set; }

        public string TipoEmisor { get; set; } //sluna: E: Empresa, P: profesional, A: administracion
        //public virtual IdentityRole TipoEmisor { get; set; }

        public string TipoReceptor { get; set; } //sluna: E: Empresa, P: profesional, A: administracion
        //public virtual IdentityRole TipoReceptor { get; set; }

        
        //sluna: relacion 1 a M con Notificacion (muchos)
        //public virtual ICollection<Notificacion> Notificaciones { get; set; }
    }

   
}