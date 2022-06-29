using Microsoft.AspNetCore.Mvc;

namespace ProductService.Api.Controllers
{
	[ApiController]
	[Route("")]
	public class HomeController : ControllerBase
	{
		[HttpGet("ping")]
		public IActionResult Ping()
		{
			return Ok("pong...");
		}
	}
}
