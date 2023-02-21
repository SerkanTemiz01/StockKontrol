using Microsoft.AspNetCore.Mvc;

namespace StockControlProject.WebUI.Areas.User.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
