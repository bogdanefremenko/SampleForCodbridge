using Microsoft.AspNetCore.Mvc;

namespace SampleForCodebridge.Web.Controllers;

[ApiController]
[Route("/ping")]
public class PingController : Controller
{
	[HttpGet]
	public IActionResult Ping()
	{
		return Content("Dogs house service. Version " + $"... Version {GetType().Assembly.GetName().Version}");
	}
}