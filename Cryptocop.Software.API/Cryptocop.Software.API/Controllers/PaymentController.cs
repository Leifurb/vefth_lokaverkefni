using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Models.Exceptions;
namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Route("")]
        //Gets all payment cards associated with the authenticated user
        public IActionResult GetStoredPaymentCards(){
            if (User.Identity == null){
                throw new IdentityException();
            }
            return Ok(_paymentService.GetStoredPaymentCards(User.Identity.Name));
        }
        [HttpPost]
        [Route("")]
        //Adds a new payment card associated with the authenticated user
        public IActionResult AddNewPaymentCard([FromBody] PaymentCardInputModel newcard){
            if (!ModelState.IsValid){
                throw new ModelFormatException("Payment card is in a wrong format");
            }
            if (User.Identity == null){
                throw new IdentityException();
            }
            _paymentService.AddPaymentCard(User.Identity.Name, newcard);
            return NoContent();
        }
    }
}