using Microsoft.AspNetCore.Mvc;
using Pizza.Models;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Authorization;

namespace Pizza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly string connectionString;

        public PizzaController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var pizza = connection.Query<pizza1>("SELECT* FROM pizzadata").ToList();
                return Ok(pizza);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
           using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT* FROM pizzadata WHERE Id= @Id";
                var pizza = connection.Query<pizza1>( query ,new { Id=id});
                if (pizza== null)
                {
                    return NotFound();
                }
                return Ok(pizza);
            }
          
        }
    
        [HttpPost]
        [Authorize]
        public IActionResult Post(pizza1 pizza)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "INSERT into pizzadata(Id,Name,Toppings,Category,Size,Price) values (@Id,@Name,@Toppings,@Category,@Size,@Price)";
                connection.Execute(query, pizza);
            }
            return Ok();
        }





        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, pizza1 pizza)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "UPDATE pizzadata SET Name=@Name,Toppings=@Toppings,Category=@Category,Size=@Size,Price=@Price WHERE Id=@Id";
                pizza.Id = id;
                connection.Execute(query, pizza);
            }
            return Ok();
        }





        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM pizzadata WHERE Id=@Id";
                connection.Execute(query, new { Id = id });
            }
            return Ok();
        }
    }
}
