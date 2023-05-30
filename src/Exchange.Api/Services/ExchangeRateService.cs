using Exchange.Api.Models;
using Exchange.Api.Models.ViewModels;
using System.Xml.Linq;

namespace Exchange.Api.Services
{
    public class ExchangeRateService
    {
        private readonly Valute _valute;
        public ExchangeRateService()
        {
            _valute = new() { CharCode = "RUB", FullName = "Российский рубль", Value = 1 };
        }
        public ExchangeRateService(Valute valute)
        {
            _valute = valute;
        }
        /// <summary>
        /// Метод для получения свежих данных о курсах от цб рф
        /// </summary>
        /// <param name="date">Дата на которую необходимо получить котировку</param>
        /// <returns>Список курсов валют по отношению к основной валюте</returns>
        public List<Valute> GetQuotes(DateTime? date)
        {
            //Устанавливаем url
            string url;
            List<Valute> result = new();
            if (!date.HasValue)
                url = @"http://www.cbr.ru/scripts/XML_daily.asp";
            else
                url = @$"http://www.cbr.ru/scripts/XML_daily.asp?date_req={date.Value.ToString("d")}";

            //Извлекаем данные полученные из ссылки
            XDocument doc = XDocument.Load(url);
            IEnumerable<XElement> valutes = doc.Descendants("Valute");
            foreach (XElement elem in valutes)
                result.Add(GetValuteFromXml(elem));


            result.Add(_valute);
            return result;
        }
        public ConvertedCurrenciesVM ConvertValute(string from, string to, List<Valute> valutes, decimal amount = 1)
        {
            Valute fromVal = valutes.FirstOrDefault(val => val.CharCode.ToUpper().Equals(from.ToUpper()));
            Valute toVal = valutes.FirstOrDefault(val => val.CharCode.ToUpper().Equals(to.ToUpper()));
            if (fromVal is null || toVal is null)
                throw new KeyNotFoundException("Валюта с таким кодом не найдена");
            ConvertedCurrenciesVM result = new ConvertedCurrenciesVM();
            result.charCodeFrom = from;
            result.charCodeTo = to;
            result.amount = amount;
            if (fromVal.Equals(_valute) || toVal.Equals(_valute))
                result.result = fromVal.Equals(_valute) ? amount / toVal.Value : fromVal.Value * amount;
            else
                result.result = fromVal.Value * amount / toVal.Value;
            return result;
        }
        /// <summary>
        /// Извлекает данные о валюте из xml
        /// </summary>
        /// <param name="xml">Валюта в сериализованном виде</param>
        /// <returns>Валюта полученная из xml</returns>
        private Valute GetValuteFromXml(XElement xml)
        {
            Valute valute = new();
            valute.Value = decimal.Parse(xml.Element("Value")!.Value) / int.Parse(xml.Element("Nominal")!.Value);
            valute.FullName = xml.Element("Name")!.Value;
            valute.CharCode = xml.Element("CharCode")!.Value;
            return valute;
        }
    }
}
