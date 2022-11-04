using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Models.Exceptions;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        //Gets all orders associated with the authenticated user
        public IActionResult GetOrders(){
            if (User.Identity == null){
                throw new IdentityException();
            }
            return Ok(_orderService.GetOrders(User.Identity.Name));
        }
        [HttpPost]
        [Route("")]
        //Adds a new order associated with the authenticated user
        public IActionResult CreateNewOrder([FromBody]  OrderInputModel order){
            if (User.Identity == null){
                throw new IdentityException();
            }
            _orderService.CreateNewOrder(User.Identity.Name, order);
            return NoContent();
        }
    }
}