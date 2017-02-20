using System.Runtime.Serialization;

namespace VLaboralApi.ViewModels.Filtros
{
   
    public abstract class SearchFilterQueryBindingModel
    {
        public int Page { get; set; }
        public int Rows { get; set; }
        public string SearchText { get; set; }
    }

    [DataContract]
    public class ValorFiltroViewModel
    {

        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Valor")]
        public string Valor { get; set; }

        [DataMember(Name = "Descripcion")]
        public string Descripcion { get; set; }

         [DataMember(Name = "Cantidad")]
        public int? Cantidad { get; set; }
    }
}