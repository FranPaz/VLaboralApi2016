namespace VLaboralApi.Models
{
    public class OfertaEstado
    {
        public int Id { get; set; }
        public string FechaEstado { get; set; }

        //iafar:relacion 1 a m con TipoEstadoOferta(1)
        public int TipoEstadoOfertaId { get; set; }
        public TipoEstadoOferta TipoEstadoOferta { get; set; }

        //iafar:relacion 1 a m con Oferta (1)
        public int OfertaId { get; set; }
        public virtual Oferta Ofertas { get; set; }

    }
}