using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.InputModels;
namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingcartService;

        public ShoppingCartController(IShoppingCartService shoppingcartService)
        {
            _shoppingcartService = shoppingcartService;
        }

        [HttpGet]
        [Route("")]
        //Gets all items within the shopping cart for current user
        public IActionResult GetCartItems()
            => Ok(_shoppingcartService.GetCartItems(User.Identity.Name));

        [HttpPost]
        [Route("")]
        //Adds an item to the shopping cart for current user
        public IActionResult AddCartItem([FromBody] ShoppingCartItemInputModel shoppingCartItemItem){
            _shoppingcartService.AddCartItem(User.Identity.Name, shoppingCartItemItem);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        //Deletes an item from the shopping cart for current user
        public IActionResult RemoveCartItem(int id){
            _shoppingcartService.RemoveCartItem(User.Identity.Name, id);
            return NoContent();
        }

        [HttpPatch]
        [Route("{id}")]
        // Updates the quantity for a shopping cart item for current user
        public IActionResult UpdateCartItemQuantity( int id, [FromBody] float quantity){
            _shoppingcartService.UpdateCartItemQuantity(User.Identity.Name, id, quantity);
            return NoContent();
        }

        [HttpDelete]
        [Route("")]
        // Clears the cart - all items within the cart should be deleted for current user
        public IActionResult ClearCart(){
            _shoppingcartService.ClearCart(User.Identity.Name);
            return NoContent();
        }
    }
}