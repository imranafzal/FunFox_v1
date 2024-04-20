using funfox_App.Models;
using funfox_App.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http;
using System.Net.Http.Headers;

namespace funfox_App.Controllers
{
    public class AdminProgramController : Controller
    {
        private readonly ILogger<AdminProgramController> _logger;
        private readonly HttpClientUtility _httpClientUtility;

        public AdminProgramController(ILogger<AdminProgramController> logger, HttpClientUtility httpClientUtility)
        {
            _logger = logger;
            _httpClientUtility = httpClientUtility;
        }

        public IActionResult Add()
        {
            return View(new funfox_App.Models.Program());
        }

        [HttpPost]
        public async Task<IActionResult> Add(funfox_App.Models.Program model)
        {
            ModelState.Remove(nameof(Models.Program.ProgramID));

            if (ModelState.IsValid)
            {

                try
                {
                    string json = JsonConvert.SerializeObject(new { model.ProgramName, model.Description, model.StartDate });

                    var result = await _httpClientUtility.PostAsync("Program/Add", json);
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

        public async Task<IActionResult> Edit(int id)
        {
            //Select Program
            funfox_App.Models.Program model = new();
            try
            {
                var result = await _httpClientUtility.GetAsync("Program/SelectProgram?ProgramID=" + id);
                model = JsonConvert.DeserializeObject<funfox_App.Models.Program>(result);
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
        public async Task<IActionResult> Update(funfox_App.Models.Program model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(model);

                    var result = await _httpClientUtility.PostAsync("Program/Update", json);
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

        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                string json = JsonConvert.SerializeObject(new { });

                var result = await _httpClientUtility.PostAsync("Program/Delete?programId=" + id, json);
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

        public async Task<IActionResult> List()
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

            return View(list);
        }
    }
}
