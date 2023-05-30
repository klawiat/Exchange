using Exchange.Api.Models;
using Exchange.Api.Services;
namespace Exchange.Tests
{
    public class ExchangeServiceTests
    {
        [Fact]
        public void CheckConvertation()
        {
            // Arrange
            ExchangeRateService service = new ExchangeRateService();
            List<Valute> valutes = new List<Valute>();
            Valute Aud = new() { CharCode = "AUD", FullName = "Австралийский доллар", Value = 1m };
            Valute Azn = new() { CharCode = "AZN", FullName = "Азербайджанский манат", Value = 1m };
            valutes.Add(Aud);
            valutes.Add(Azn);
            decimal expected = 1m;
            // Act
            Api.Models.ViewModels.ConvertedCurrenciesVM result = service.ConvertValute("AUD", "AZN", valutes, 1);
            // Assert
            Assert.Equal(expected, result.result);
        }
        [Theory]
        [InlineData("AUD")]
        [InlineData("GBP")]
        public void NotFoundValute(string name)
        {
            // Arrange
            ExchangeRateService service = new ExchangeRateService();
            List<Valute> valutes = new List<Valute>();
            Valute randomValute = new() { CharCode = name + name, FullName = "Австралийский доллар", Value = 1m };
            Valute Azn = new() { CharCode = "AZN", FullName = "Азербайджанский манат", Value = 1m };
            valutes.Add(randomValute);
            valutes.Add(Azn);
            Type expected = typeof(KeyNotFoundException);
            // Act
            Action result = () => service.ConvertValute(name, "AZN", valutes, 1);
            // Assert
            Assert.Throws(expected, result);
        }
    }
}