using Microsoft.AspNetCore.Mvc;
using Api.AddressApiClient;
using Api.AddressApiClient.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AddressClient _addressClient;

        public AddressController(AddressClient client) =>
                    _addressClient = client;

        [HttpGet]
        [Produces(typeof(List<Address>))]
        public async Task<IActionResult> GetAddresses()
        {
            var addresses = await _addressClient.Api.Address.GetAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        [ProducesResponseType<Address>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAddress(int id)
        {
            var address = await _addressClient.Api.Address[id].GetAsync();
            if(address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }

        [HttpPost]
        [Produces(typeof(int))]
        public async Task<IActionResult> CreateAddress(RequestAddress address)
        {
            var createdAddressId = await _addressClient.Api.Address.PostAsync(address);
            return Ok(createdAddressId);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAddress(int id, RequestAddress address)
        {
            await _addressClient.Api.Address[id].PutAsync(address);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            await _addressClient.Api.Address[id].DeleteAsync();
            return NoContent();
        }
    }
}
