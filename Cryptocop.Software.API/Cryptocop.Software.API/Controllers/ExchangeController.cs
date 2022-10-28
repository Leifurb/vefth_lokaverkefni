using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/exchanges")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeServices;

        public ExchangeController(IExchangeService exchangeServices)
        {
            _exchangeServices = exchangeServices;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllExchanges([FromQuery] int PageNumber = 1)
            => Ok(_exchangeServices.GetExchanges(PageNumber).Result);
        
    }
}