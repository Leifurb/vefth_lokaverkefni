using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Models.Exceptions;

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
        public IActionResult GetCartItems(){
            if (User.Identity == null){
                throw new IdentityException();
            }
            return Ok(_shoppingcartService.GetCartItems(User.Identity.Name));
        }

        [HttpPost]
        [Route("")]
        //Adds an item to the shopping cart for current user
        public async Task<IActionResult> AddCartItem([FromBody] ShoppingCartItemInputModel shoppingCartItemItem){
            if (User.Identity == null){
                throw new IdentityException();
            }
            await _shoppingcartService.AddCartItem(User.Identity.Name, shoppingCartItemItem);
            return new ObjectResult("ShoppingCartItem added to database") { StatusCode = StatusCodes.Status201Created };
        }

        [HttpDelete]
        [Route("{id}")]
        //Deletes an item from the shopping cart for current user
        public IActionResult RemoveCartItem(int id){
            if (User.Identity == null){
                throw new IdentityException();
            }
            _shoppingcartService.RemoveCartItem(User.Identity.Name, id);
            return NoContent();
        }

        [HttpPatch]
        [Route("{id}")]
        // Updates the quantity for a shopping cart item for current user
        public IActionResult UpdateCartItemQuantity( int id, [FromBody] float quantity){
            if (User.Identity == null){
                throw new IdentityException();
            }
            _shoppingcartService.UpdateCartItemQuantity(User.Identity.Name, id, quantity);
            return NoContent();
        }

        [HttpDelete]
        [Route("")]
        // Clears the cart - all items within the cart should be deleted for current user
        public IActionResult ClearCart(){
            if (User.Identity == null){
                throw new IdentityException();
            }
            _shoppingcartService.ClearCart(User.Identity.Name);
            return NoContent();
        }
    }
}