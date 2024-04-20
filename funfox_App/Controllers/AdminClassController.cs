using funfox_App.Models;
using funfox_App.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace funfox_App.Controllers
{
    public class AdminClassController : Controller
    {
        private readonly ILogger<AdminClassController> _logger;
        private readonly HttpClientUtility _httpClientUtility;

        public AdminClassController(ILogger<AdminClassController> logger, HttpClientUtility httpClientUtility)
        {
            _logger = logger;
            _httpClientUtility = httpClientUtility;
        }

        public async Task<IActionResult> Add()
        {
            List<Models.Program> list = await FillProgramsList();
            ViewBag.programs = new SelectList(list, "ProgramID", "ProgramName");
            return View(new funfox_App.Models.Class());
        }

        [HttpPost]
        public async Task<IActionResult> Add(funfox_App.Models.Class model)
        {
            ModelState.Remove(nameof(Models.Class.ProgramID));

            if (ModelState.IsValid)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(new { model.ClassName, model.GradeLevel, model.Timings, model.MaxClassSize, model.ProgramID });   //using Newtonsoft.Json

                    var result = await _httpClientUtility.PostAsync("Class/Add", json);
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

            List<Models.Program> list = await FillProgramsList();
            ViewBag.programs = new SelectList(list, "ProgramID", "ProgramName");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            funfox_App.Models.Class model = new();
            try
            {
                var result = await _httpClientUtility.GetAsync("Class/Get?ClassId=" + id);
                model = JsonConvert.DeserializeObject<funfox_App.Models.Class>(result);
                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }

            List<Models.Program> list = await FillProgramsList();
            ViewBag.programs = new SelectList(list, "ProgramID", "ProgramName");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(funfox_App.Models.Class model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    string json = JsonConvert.SerializeObject(model);

                    var result = await _httpClientUtility.PostAsync("Class/Update", json);
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
                string json = JsonConvert.SerializeObject(new{ });

                var result = await _httpClientUtility.PostAsync("Class/Delete?classId=" + id, json);
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


        private async Task<List<Models.Program>> FillProgramsList()
        {
            List<Models.Program> list = new();
            try
            {
                string json = JsonConvert.SerializeObject(new { });

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

        public async Task<IActionResult> List()
        {
            List<Models.Class> list = new();
            try
            {
                var result = await _httpClientUtility.GetAsync("Class/List");
                list = JsonConvert.DeserializeObject<List<Models.Class>>(result);
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
