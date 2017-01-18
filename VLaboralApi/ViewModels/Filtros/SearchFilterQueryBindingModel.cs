namespace VLaboralApi.ViewModels.Filtros
{
    public abstract class SearchFilterQueryBindingModel
    {
        public int Page { get; set; }
        public int Rows { get; set; }
        public string SearchText { get; set; }
    }

    public class ValorFiltroViewModel
    {
       
        public int Id { get; set; }

        public string Valor { get; set; }

        public string Descripcion { get; set; }

        public int? Cantidad { get; set; }
    }
}