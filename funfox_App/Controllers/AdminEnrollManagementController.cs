using funfox_App.Models;
using funfox_App.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace funfox_App.Controllers
{
    public class AdminEnrollManagementController : Controller
    {
        private readonly ILogger<AdminEnrollManagementController> _logger;
        private readonly HttpClientUtility _httpClientUtility;

        public AdminEnrollManagementController(ILogger<AdminEnrollManagementController> logger, HttpClientUtility httpClientUtility)
        {
            _logger = logger;
            _httpClientUtility = httpClientUtility;
        }

        public async Task<IActionResult> EnrollmentList()
        {
            List<Models.EnrollmentDetails> list = new();

            try
            {
                string json = JsonConvert.SerializeObject(new { });

                var result = await _httpClientUtility.GetAsync("Enrollment/EnrollmentsList");
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

    }
}
