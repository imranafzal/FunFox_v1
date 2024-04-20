using funfox_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using funfox_App.Utility;

namespace funfox_App.Controllers
{

    public class UserController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private readonly HttpClientUtility _httpClientUtility;

        public UserController(ILogger<UserController> logger, HttpClientUtility httpClientUtility)
        {
            _logger = logger;
            _httpClientUtility = httpClientUtility;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Bearer_Tokens")))
                return RedirectToAction("Index", "Home");

            //ViewBag.JavaScriptFunction = string.Format("setSelection('{0}')", "Index");

            return View();
        }

        public async Task<IActionResult> Profile()
        {
            Models.User model = new();
            try
            {
                string loginName = HttpContext.Session.GetString("IdentityUsername");

                var result = await _httpClientUtility.GetAsync("User/FindUserByLoginName?loginName=" + loginName);
                model = JsonConvert.DeserializeObject<Models.User>(result);
                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }

            ViewBag.JavaScriptFunction = string.Format("setSelection('{0}')", "Profile");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EnrollClass(Models.Enrollment model)
        {
            ModelState.Remove(nameof(Models.Enrollment.EnrollmentID));
            ModelState.Remove(nameof(Models.Enrollment.UserID));
            if (ModelState.IsValid)
            {
                try
                {
                    int userId = await getUserId();
                    string json = JsonConvert.SerializeObject(new { });

                    var result = await _httpClientUtility.PostAsync(string.Format("Enrollment/EnrollForClass?userId={0}&classId={1}", userId, model.ClassID), json);
                    
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

            return View(model);
        }

        public async Task<IActionResult> EnrollClass()
        {

            List<Models.Program> listp = await FillProgramsList();
            ViewBag.programs = new SelectList(listp, "ProgramID", "ProgramName");

            List<Models.Class> list = new();
            ViewBag.classes = new SelectList(list, "ClassID", "ClassName");

            ViewBag.JavaScriptFunction = string.Format("setSelection('{0}')", "EnrollClass");

            string host = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            ViewData["BaseUrl"] = host;

            return View();
        }

        [HttpPost, ActionName("GetClassesDetails")]
        public JsonResult GetClassesDetails(string classID)
        {
            Models.Class item = new();
            try
            {
                string loginName = HttpContext.Session.GetString("IdentityUsername");

                var result =  _httpClientUtility.GetAsync("Class/Get?ClassID=" + classID).Result;
                item = JsonConvert.DeserializeObject<Models.Class>(result);
                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }

            return Json(item);
        }

        private async Task<int> getUserId() {

            Models.User item = new();
            try
            {
                string loginName = HttpContext.Session.GetString("IdentityUsername");

                var result = await _httpClientUtility.GetAsync("User/FindUserByLoginName?loginName=" + loginName);
                item = JsonConvert.DeserializeObject<Models.User>(result);
                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }

            return item.UserID;
        }

        [HttpPost, ActionName("GetClassesByProgramId")]
        public JsonResult GetClassesByProgramId(string programID)
        {
            List<Models.Class> list = new();

            if (int.TryParse(programID, out int pID))
            {
                list = FillClassesList(pID).Result;
                ViewBag.classes = new SelectList(list, "ClassID", "ClassName");
            }

            return Json(list);
        }

        private async Task<List<Models.Class>> FillClassesList(int programID)
        {
            List<Models.Class> list = new();
            try
            {
                var result = await _httpClientUtility.GetAsync("Class/FilteredList?ProgramId=" + programID);
                list = JsonConvert.DeserializeObject<List<Models.Class>>(result);
                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }

            return list;
        }

        private async Task<List<Models.Program>> FillProgramsList()
        {
            List<Models.Program> list = new();

            try
            {
                var result = await _httpClientUtility.GetAsync("Program/List");
                list = JsonConvert.DeserializeObject<List<Models.Program>>(result);
                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }

            return list;
        }


        public async Task<IActionResult> EnrollmentList()
        {
            List<Models.EnrollmentDetails> list = new();

            try
            {
                int userId = await getUserId();
                var result = await _httpClientUtility.GetAsync("Enrollment/UserEnrolledClasses?userID=" + userId);
                list = JsonConvert.DeserializeObject<List<Models.EnrollmentDetails>>(result);
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

        public async Task<IActionResult> Delete(int enrollmentId)
        {

            try
            {
                string json = JsonConvert.SerializeObject(new { });

                var result = await _httpClientUtility.PostAsync("Enrollment/CancelEnrollment?enrollmentID=" + enrollmentId, json);
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

        public IActionResult Signout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
