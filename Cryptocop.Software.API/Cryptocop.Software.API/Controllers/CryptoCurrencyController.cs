using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cryptocurrencies")]
    public class CryptoCurrencyController : ControllerBase
    {
        private readonly ICryptoCurrencyService _cryptocurrencyService;

        public CryptoCurrencyController(ICryptoCurrencyService cryptocurrencyService)
        {
            _cryptocurrencyService = cryptocurrencyService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAvailableCryptocurrencies()
            => Ok(_cryptocurrencyService.GetAvailableCryptocurrencies().Result);
    }
}
