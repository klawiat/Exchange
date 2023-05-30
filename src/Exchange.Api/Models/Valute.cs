namespace Exchange.Api.Models
{
    public class Valute
    {
        public string CharCode { get; set; }
        public string FullName { get; set; }
        public decimal Value { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj is Valute valute)
                return valute.CharCode.Equals(CharCode) && valute.FullName.Equals(FullName) && valute.Value.Equals(Value);
            return false;
        }
    }
}
