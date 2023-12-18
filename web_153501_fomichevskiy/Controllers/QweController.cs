using Microsoft.AspNetCore.Mvc;

namespace web_153501_fomichevskiy.Controllers
{
	public class QweController : Controller
	{
		public IActionResult Index()
		{
			return View((123, 123));
		}
	}
}
