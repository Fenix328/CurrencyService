using CurrencyService.Controllers;
using CurrencyService.Interfaces;
using CurrencyService.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections;

namespace CurrencyService.Tests.Unit.Controllers
{
    public class CurrencyControllerTests
    {
        private readonly CurrencyController _sut;

        private readonly Mock<ICurrencyService> _currencyService;

        public CurrencyControllerTests()
        {
            _currencyService = new Mock<ICurrencyService>();

            _sut = new CurrencyController(_currencyService.Object);
        }

        [Theory]
        [ClassData(typeof(GetCurrencyClassData))]
        public async Task GetCurrency_ShouldReturnOk_WhenCurrencyExists(DateTime setupExchangeDate, Currency currency, DateTime exchangeDate)
        {
            // Arrange
            _currencyService.Setup(cs => cs.GetCurrencyAsync(It.IsAny<int>(), setupExchangeDate)).ReturnsAsync(currency);

            // Act
            var result = (OkObjectResult) await _sut.GetCurrency(It.IsAny<int>(), exchangeDate);

            // Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be(currency);
        }

        [Fact]
        public async Task GetCurrency_ShouldReturnNotFound_WhenCurrencyDoesntExists()
        {
            // Arrange
            int currencyCode = 36;
            string errorMessage = $"Валюту з кодом {currencyCode} не знайдено!";
            _currencyService.Setup(cs => cs.GetCurrencyAsync(It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(default(Currency));

            // Act
            var result = (NotFoundObjectResult) await _sut.GetCurrency(currencyCode, It.IsAny<DateTime>());

            // Assert
            result.StatusCode.Should().Be(404);
            result.Value.As<string>().Should().Be(errorMessage);
        }

        [Theory]
        [ClassData(typeof(GetCurrencyListClassData))]
        public async Task GetCurrencyList_ShouldReturnOk(DateTime setupExchangeDate, List<Currency> currencies, DateTime exchangeDate)
        {
            // Arrange
            _currencyService.Setup(cs => cs.GetCurrencyListAsync(setupExchangeDate)).ReturnsAsync(currencies);

            // Act
            var result = (OkObjectResult)await _sut.GetCurrencyList(exchangeDate);

            // Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(currencies);
        }

        private class GetCurrencyClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    It.IsAny<DateTime>(),
                    new Currency()
                    {
                        Code = 36,
                        Key = "AUD",
                        Name = "Австралійський долар",
                        Value = 24.6797M,
                        ExchangeDateStr = DateTime.Now.ToString("dd.MM.yyyy")
                    },
                    It.IsAny<DateTime>()
                };
                yield return new object[]
                {
                    It.Is<DateTime>(d => d.Date == DateTime.Now.Date),
                    new Currency()
                    {
                        Code = 36,
                        Key = "AUD",
                        Name = "Австралійський долар",
                        Value = 24.6797M,
                        ExchangeDateStr = DateTime.Now.ToString("dd.MM.yyyy")
                    },
                    default(DateTime?)
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class GetCurrencyListClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    It.IsAny<DateTime>(),
                    new List<Currency>(),
                    It.IsAny<DateTime>()
                };

                yield return new object[]
                {
                    It.IsAny<DateTime>(),
                    new List<Currency>()
                    {
                        new Currency
                        {
                            Code = 36,
                            Key = "AUD",
                            Name = "Австралійський долар",
                            Value = 24.6797M,
                            ExchangeDateStr = DateTime.Now.ToString("dd.MM.yyyy")
                        }
                    },
                    It.IsAny<DateTime>()
                };

                yield return new object[]
                {
                    It.Is<DateTime>(d => d.Date == DateTime.Now.Date),
                    new List<Currency>()
                    {
                        new Currency
                        {
                            Code = 36,
                            Key = "AUD",
                            Name = "Австралійський долар",
                            Value = 24.6797M,
                            ExchangeDateStr = DateTime.Now.ToString("dd.MM.yyyy")
                        }
                    },
                    default(DateTime?)
                };

            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
