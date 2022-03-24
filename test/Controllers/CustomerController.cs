using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDbContext context;

        public CustomerController(CustomerDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<List<Customer>> Get()
        {
            return await context.Customers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Customer> Get(int id)
        {
            return await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), customer);
        }
    }
}
