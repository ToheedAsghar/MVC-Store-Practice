using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
			//if (retVal) { RedirectToAction("Index", "Store"); }
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
