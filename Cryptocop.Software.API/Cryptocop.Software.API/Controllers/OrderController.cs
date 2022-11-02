using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.InputModels;

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
            return Ok(_orderService.GetOrders(User.Identity.Name));
        }
        [HttpPost]
        [Route("")]
        //Adds a new order associated with the authenticated user
        public IActionResult CreateNewOrder([FromBody]  OrderInputModel order){
            _orderService.CreateNewOrder(User.Identity.Name, order);
            return NoContent();
        }
    }
}