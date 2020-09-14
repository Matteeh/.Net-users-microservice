namespace UserApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using UserApi.Services;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("api/users")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;
        public AuthController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(string Id)
        {
            if (Id == null)
            {
                return BadRequest("No ID provided");
            }

            try
            {
                var user = await _cosmosDbService.GetUsersAsync($"Select * From Users u WHERE u.id = {Id}");
                if (user.FirstOrDefault() != null)
                {
                    return Ok(user.FirstOrDefault());
                }
                return NotFound("User not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("")]
        public async Task<ActionResult> ListAll()
        {
            try
            {
                var users = await _cosmosDbService.GetUsersAsync($"SELECT * FROM Users u");
                if (users.FirstOrDefault() != null)
                {
                    return Ok(users);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return Ok(e.Message);
            }
        }




    }
}