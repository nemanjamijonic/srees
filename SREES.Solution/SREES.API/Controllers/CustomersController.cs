using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Customers;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerApplicationService _customerApplicationService;

        public CustomersController(ICustomerApplicationService customerApplicationService)
        {
            _customerApplicationService = customerApplicationService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponsePackage<List<CustomerDataOut>>>> GetAllCustomers()
        {
            var result = await _customerApplicationService.GetAllCustomers();
            return Ok(result);
        }

        [HttpGet("getAllForSelect")]
        public async Task<ActionResult<ResponsePackage<List<CustomerSelectDataOut>>>> GetAllCustomersForSelect()
        {
            var result = await _customerApplicationService.GetAllCustomersForSelect();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsePackage<CustomerDataOut?>>> GetCustomerById(int id)
        {
            var result = await _customerApplicationService.GetCustomerById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ResponsePackage<CustomerDataOut?>>> CreateCustomer([FromBody] CustomerDataIn customerDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerApplicationService.CreateCustomer(customerDataIn);
            if (result.Data == null)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetCustomerById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponsePackage<CustomerDataOut?>>> UpdateCustomer(int id, [FromBody] CustomerDataIn customerDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerApplicationService.UpdateCustomer(id, customerDataIn);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponsePackage<string>>> DeleteCustomer(int id)
        {
            var result = await _customerApplicationService.DeleteCustomer(id);
            if (result.Data == null && result.Message.Contains("nije pronađen"))
                return NotFound(result);

            return Ok(result);
        }
    }
}
