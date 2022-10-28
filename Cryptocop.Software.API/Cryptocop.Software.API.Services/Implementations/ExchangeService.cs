using System.Threading.Tasks;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Services.Helpers;

using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System;
namespace Cryptocop.Software.API.Services.Implementations
{
    public class ExchangeService : IExchangeService
    {
        private HttpClient _httpClient;

        public ExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)
        {
            _httpClient.DefaultRequestHeaders.Add("x-messari-api-key", "7c5f8d9e-ea09-404d-abfe-f8a39bd9ee2a");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await _httpClient.GetAsync("v1/markets?PageNumber=" + pageNumber.ToString()+ "&fields=exchange_id,exchange_name,exchange_slug,base_asset_symbol,price_usd,last_trade_at");
            var res = await HttpResponseMessageExtensions.DeserializeJsonToList<ExchangeDto>(response, true);
            var env = new Envelope<ExchangeDto>();
            env.PageNumber = pageNumber;
            env.Items = res;
            return env;
        }
    }
}