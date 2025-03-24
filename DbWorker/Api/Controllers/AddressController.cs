using Api.DTOs;
using Api.Libraries.AddressLibrary.Models;
using Api.Libraries.AddressLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController(IAddressRepository addressRepository) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType<IEnumerable<Address>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAddresses()
        {
            try
            {
                var addresses = await addressRepository.GetAllAddressesAsync();
                return Ok(addresses);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType<Address>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAddress(int id)
        {
            try
            {
                var address = await addressRepository.GetAddressByIdAsync(id);
                return Ok(address);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAddress(RequestAddress address)
        {
            try
            {
                var rowId = await addressRepository.AddAddressAsync(address);
                return Ok(rowId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAddress(int id, RequestAddress address)
        {
            await addressRepository.UpdateAddressAsync(id, address);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                _ = await addressRepository.GetAddressByIdAsync(id);

                await addressRepository.DeleteAddressAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }
    }
}
