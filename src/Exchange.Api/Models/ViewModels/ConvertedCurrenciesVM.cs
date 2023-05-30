namespace Exchange.Api.Models.ViewModels
{
    public class ConvertedCurrenciesVM
    {
        public string charCodeFrom { get; set; }
        public decimal amount { get; set; }
        public string charCodeTo { get; set; }
        public decimal result { get; set; }
    }
}
