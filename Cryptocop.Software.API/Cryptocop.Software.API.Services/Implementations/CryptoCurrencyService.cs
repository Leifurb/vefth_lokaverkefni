using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Services.Helpers;

using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Linq;
namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private HttpClient _httpClient;

        public CryptoCurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CryptoCurrencyDto>> GetAvailableCryptocurrencies()
        {
            while (true){
                int pageNumber = 1;
                int limit = 500;
                _httpClient.DefaultRequestHeaders.Add("x-messari-api-key", "7c5f8d9e-ea09-404d-abfe-f8a39bd9ee2a");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await _httpClient.GetAsync("v2/assets"+ "?pageNumber="+pageNumber+"&limit="+ limit +"&fields=id,symbol,name,slug,metrics/market_data/price_usd,profile/general/overview/project_details");
                var res = await HttpResponseMessageExtensions.DeserializeJsonToList<CryptoCurrencyDto>(response, true);
                List<CryptoCurrencyDto> selectedCurrency = new List<CryptoCurrencyDto>();
                var currencies = new List<string>() { "ETH", "USDT", "BTC", "XMR" };

                return res.Where(i => currencies.Contains(i.Symbol));
            }
        }
    }
}