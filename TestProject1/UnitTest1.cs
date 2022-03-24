using System;
using Xunit;
using test;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using test.Controllers;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.True(1 == 1);
        }

        [Fact]
        public async Task CustomerIntegrationTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            var builder = new DbContextOptionsBuilder<CustomerDbContext>().UseSqlServer(config.GetConnectionString("DefaultConnection"));
            var context = new CustomerDbContext(builder.Options);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            context.Customers.RemoveRange(context.Customers);
            await context.SaveChangesAsync();

            var controller = new CustomerController(context);

            await controller.AddCustomer(new Customer() { Name = "jsajfdsjfc;sadm" });

            var customers = await controller.Get();

            Assert.Single(customers);
            Assert.Equal("jsajfdsjfc;sadm", customers[0].Name);

            //using var httpCilent = new HttpClient();
            //var customer = new Customer() { Name = "mohamed" };
            //var stringContent = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            // var output = await httpCilent.PostAsync("https://localhost:5001/api/Customer", stringContent);
            //Assert.True(output.StatusCode == System.Net.HttpStatusCode.Created);
            //var data = await httpCilent.GetAsync("https://localhost:5001/api/Customer");
            //Assert.True(data.StatusCode == System.Net.HttpStatusCode.OK);
        }
    }
}
