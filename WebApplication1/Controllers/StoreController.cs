using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
			/* If user visits the site first time, then it's inital time and date is stored in cookies. When it visits seconds time
             the date and time is updated and the last date and time is shown on the Index page. */
			CookieOptions options = new()
			{
				Expires = System.DateTime.Now.AddDays(1)
			};

			string Data = string.Empty;
            if (HttpContext.Request.Cookies.ContainsKey("flag"))
            {
                Data = "Last Visited: " + HttpContext.Request.Cookies["flag"];
				HttpContext.Response.Cookies.Append("flag", System.DateTime.Now.ToString(), options);
			}
			else
            {
                HttpContext.Response.Cookies.Append("flag", System.DateTime.Now.ToString(), options);
            }
            return View("Index", Data);
        }

        /* Action Method to delete all Cookies. */
        public ActionResult DeleteCookies()
        {
            HttpContext.Response.Cookies.Delete("flag");
            return RedirectToAction("Index", "Store");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string? un, string? pswd)
        {
            UserRepository userRepo = new();
            User user = new(-1, un, pswd);
            bool retVal = userRepo.Login(user);
            if (retVal)
            {
                return RedirectToAction("Index", "Store");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string? un, string? pswd)
        {
            User user = new(-1, un, pswd);
            UserRepository repo = new();
            bool retVal = repo.Register(user);
			if (retVal)
			{
				return RedirectToAction("Login", "Store");
			}
			else
			{
				return View();
			}
        }

		public ViewResult UserById(int id)
		{
			UserRepository userRepo = new();
			var user = userRepo.GetUser(id);
			return View(user);
		}

	}
}
