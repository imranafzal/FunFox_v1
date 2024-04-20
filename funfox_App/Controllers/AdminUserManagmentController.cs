using funfox_App.Models;
using funfox_App.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace funfox_App.Controllers
{
    public class AdminUserManagmentController : Controller
    {
        private readonly ILogger<AdminUserManagmentController> _logger;
        private readonly HttpClientUtility _httpClientUtility;

        public AdminUserManagmentController(ILogger<AdminUserManagmentController> logger, HttpClientUtility httpClientUtility)
        {
            _logger = logger;
            _httpClientUtility = httpClientUtility;
        }

        public async Task<IActionResult> Add()
        {
            return View(new funfox_App.Models.User());
        }

        [HttpPost]
        public async Task<IActionResult> Add(funfox_App.Models.User model)
        {
            ModelState.Remove(nameof(Models.User.UserID));
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
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
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

            return View(model);
        }

        public async Task<IActionResult> List()
        {
            List<Models.User> list = new();
            try
            {
                var result = await _httpClientUtility.GetAsync("User/List");
                list = JsonConvert.DeserializeObject<List<Models.User>>(result);
                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }

            return View(list);
        }

        public async Task<IActionResult> Edit(int userId)
        {

            funfox_App.Models.User model = new();
            try
            {
                var result = await _httpClientUtility.GetAsync("User/FindUserById?userId=" + userId);
                model = JsonConvert.DeserializeObject<funfox_App.Models.User>(result);
                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Update(funfox_App.Models.User model)
        {
            ModelState.Remove(nameof(Models.User.Password));
            if (ModelState.IsValid)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(model);

                    var result = await _httpClientUtility.PostAsync("User/UpdateProfile", json);
                    ViewBag.Message = result;
                    _logger.LogInformation(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    ViewBag.IsError = true;
                    ViewBag.Message = ex.Message;
                }

            }

            return View("Message", ViewBag);
        }


        public async Task<IActionResult> Delete(int userId)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new { });

                var result = await _httpClientUtility.PostAsync("User/Delete?userId=" + userId, json);
                ViewBag.Message = result;
                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }

            return View("Message", ViewBag);
        }



        public async Task<IActionResult> ResetDashboard()
        {
            try
            {
                string json = JsonConvert.SerializeObject(new { });
                var result = await _httpClientUtility.PostAsync("Admin/Reset", json);
                ViewBag.Message = result;
                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }

            return View("Message", ViewBag);
        }


    }
}
