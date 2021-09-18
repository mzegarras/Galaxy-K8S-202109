using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CustomerApi.Service;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {

         private readonly ILogger<CustomerController> _logger;
         private readonly ICustomerService _customerService;
         public CustomerController(ILogger<CustomerController> logger,
                                ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

         [HttpGet]
        public IEnumerable<Models.Customer> Get()=> _customerService.ListAll();
        
        [HttpPost]
        public ActionResult<Models.Customer> Create(Models.Customer customer)
        {
            _customerService.Create(customer);

            return CreatedAtRoute("GetCustomer", new { id = customer.Id.ToString() }, customer);
        }

        [HttpGet("{id:length(24)}", Name = "GetCustomer")]
        public ActionResult<Models.Customer> Get(string id)
        {
            var customer = _customerService.ListById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }




    }
}