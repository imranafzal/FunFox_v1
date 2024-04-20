using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace funfox_App.Controllers
{
    
    public class AdminController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        public AdminController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Bearer_Tokens")))
                return RedirectToAction("Index", "Home");

            ViewBag.JavaScriptFunction = string.Format("setSelection('{0}')", "Index");
            return View();
        }

        public IActionResult Settings()
        {

            ViewBag.JavaScriptFunction = string.Format("setSelection('{0}')", "Settings");
            return View();
        }

        public IActionResult Users()
        {
            ViewBag.JavaScriptFunction = string.Format("setSelection('{0}')", "Users");
            return View();
        }

        public IActionResult Program()
        {
            ViewBag.JavaScriptFunction = string.Format("setSelection('{0}')", "Program");
            return View();
        }

        

        public IActionResult Class()
        {
            ViewBag.JavaScriptFunction = string.Format("setSelection('{0}')", "Class");
            return View();
        }

        public IActionResult Enrollments()
        {
            ViewBag.JavaScriptFunction = string.Format("setSelection('{0}')", "Enrollments");
            return View();
        }

        public IActionResult Payments()
        {
            ViewBag.JavaScriptFunction = string.Format("setSelection('{0}')", "Payments");
            return View();
        }


        public IActionResult Signout() 
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
