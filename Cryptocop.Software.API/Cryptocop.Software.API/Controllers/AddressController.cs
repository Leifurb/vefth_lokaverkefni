using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.InputModels;
using System;
using Microsoft.AspNetCore.Identity;
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
            return Ok(_addressService.GetAllAddresses(User.Identity.Name));
        }
        [HttpPost]
        [Route("")]
        //Adds a new address associated with authenticated user, see
        public IActionResult AddAddress([FromBody] AddressInputModel address){
            _addressService.AddAddress(User.Identity.Name, address);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteAddress(int id){
            _addressService.DeleteAddress(User.Identity.Name, id);
            return NoContent();
        }
    }
}