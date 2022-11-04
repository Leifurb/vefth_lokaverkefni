using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Models.Exceptions;
namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        [Route("")]
        //Gets all addresses associated with authenticated user
        public IActionResult GetAllAddresses()
        {
            if (User.Identity == null){
                throw new IdentityException();
            }
            return Ok(_addressService.GetAllAddresses(User.Identity.Name));
        }
        [HttpPost]
        [Route("")]
        //Adds a new address associated with authenticated user, see
        public IActionResult AddAddress([FromBody] AddressInputModel address){
            if (!ModelState.IsValid){
                throw new ModelFormatException("Adddress model is in a wrong format");
            }
            if (User.Identity == null){
                throw new IdentityException();
            }
            _addressService.AddAddress(User.Identity.Name, address);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteAddress(int id){
            if (User.Identity == null){
                throw new IdentityException();
            }
            _addressService.DeleteAddress(User.Identity.Name, id);
            return NoContent();
        }
    }
}