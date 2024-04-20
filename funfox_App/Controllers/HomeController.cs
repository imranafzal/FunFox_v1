using funfox_App.Models;
using funfox_App.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace funfox_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClientUtility _httpClientUtility;

        public HomeController(ILogger<HomeController> logger, HttpClientUtility httpClientUtility)
        {
            _logger = logger;
            _httpClientUtility = httpClientUtility;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Models.User model)
        {
            ModelState.Remove(nameof(Models.User.FirstName));
            ModelState.Remove(nameof(Models.User.LastName));
            ModelState.Remove(nameof(Models.User.Email));

            if (ModelState.IsValid)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(new { email = model.Username, model.Password });   //using Newtonsoft.Json
                                                                                                                 // Convert YourModel to JSON string
                    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await _httpClientUtility.PostAsync("login", httpContent);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        ViewBag.IsError = true;
                    string result  = await response.Content.ReadAsStringAsync();
                    ViewBag.Message = result;

                    if (ViewBag.IsError is null)
                    {
                        HttpContext.Session.SetString("Bearer_Tokens", result);
                        HttpContext.Session.SetString("IdentityUsername", model.Username);
                        LoginSuccess tokens = JsonConvert.DeserializeObject<LoginSuccess>(result);
                        _httpClientUtility.SetBearerToken(tokens.accessToken);

                        if (model.Username == "portaladmin@gmail.com")
                            return RedirectToAction("Index", "Admin");
                        else
                            return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        var _error = JsonConvert.DeserializeObject<RootError>(result);
                        ViewBag.Message = "Error Code:"+ _error.type + Environment.NewLine;
                        ViewBag.Message += "Error Detail:" + _error.detail + Environment.NewLine;
                        ViewBag.Message += "link:" + _error.status.ToString() + ": Error" + Environment.NewLine;
                        _logger.LogError(result);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IsError = true;
                    ViewBag.Message = ex.Message;
                    _logger.LogError(ex.Message);
                }

            }
            else
            {
                ViewBag.IsError = true;
                ViewBag.Message = "Enter Username and Password";
            }

            ViewBag.JavaScriptFunction = string.Format("showtab('{0}')", "signIn");
            return View("Index",model);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Models.User model)
        {
            ModelState.Remove(nameof(Models.User.Username));
            if (ModelState.IsValid)
            {

                try
                {
                    model.Username = model.Email.Substring(0, model.Email.IndexOf('@'));
                    string json = JsonConvert.SerializeObject(new { model.Email, model.Password });   //using Newtonsoft.Json
                    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    var response = await _httpClientUtility.PostAsync("register", httpContent);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        ViewBag.IsError = true;
                    string result = await response.Content.ReadAsStringAsync();
                    ViewBag.Message = result;

                    if (ViewBag.IsError is null)
                    {
                        json = JsonConvert.SerializeObject(model);
                        httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                        response = await _httpClientUtility.PostAsync("user/add", httpContent);
                        if(response.StatusCode != System.Net.HttpStatusCode.OK)
                            ViewBag.IsError = true;
                        result = await response.Content.ReadAsStringAsync();
                        ViewBag.Message = result;

                    }
                    else
                    {
                        var _error = JsonConvert.DeserializeObject<RootError>(result);
                        ViewBag.Message = "Error Code:" + _error.status.ToString() + Environment.NewLine;
                        ViewBag.Message += "Error Detail:" + _error.detail + Environment.NewLine;
                        ViewBag.Message += "link:" + _error.type + Environment.NewLine;
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in _error.errors.DuplicateUserName)
                        {
                            sb.AppendLine(item);
                        }
                        ViewBag.Message += "Error:" + sb.ToString() + Environment.NewLine;
                        _logger.LogError(result);
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.IsError = true;
                    ViewBag.Message = ex.Message;
                    _logger.LogError(ex.Message);
                }


            }
            else
            {
                ViewBag.IsError = true;
                ViewBag.Message = "Validation errors on page.";
            }

            ViewBag.JavaScriptFunction = string.Format("showtab('{0}')", "signUpTab");

            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
