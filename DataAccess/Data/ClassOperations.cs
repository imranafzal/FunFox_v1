using DataAccess.DbAccess;
using DataAccess.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class ClassOperations : IClassOperations
    {
        private readonly ISqlDataAccess _dbo;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private ILogger<ClassOperations> _logger;

        public ClassOperations(ISqlDataAccess dbo, IConfiguration config, IMemoryCache cache,ILogger<ClassOperations> logger)
        {
            _dbo = dbo;
            _config = config;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> Add(Class model)
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("InsertClass", new { model.ClassName,model.GradeLevel,model.Timings,model.MaxClassSize,model.ProgramID });
                message = "Class saved successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }

            return message;
        }

        public async Task<string> Delete(int classId)
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("DeleteClass", new { ClassId = classId});
                message = "Class and all dependent data has been deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }

            return message;
        }

        public async Task<Class> Get(int ClassId)
        {
            Class item = new();
            try
            {

               var list = await _dbo.LoadData<Class, dynamic>("GetClassByID", new { ClassId });
                item = list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return item;
        }

        public async Task<List<Class>> FilteredList(int _ProgramId)
        {
            List<Class>? list = new();
            try
            {
                IEnumerable<Class> classesList = await _dbo.LoadData<Class, dynamic>("SelectFilteredClasses", new { ProgramId = _ProgramId });
                list = classesList.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return list;
        }
        public async Task<List<Class>> List()
        {
            List<Class>? list = new();
            try
            {

                IEnumerable<Class> classesList = await _dbo.LoadData<Class, dynamic>("SelectClass", new { });
                list = classesList.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return list;
        }

        public async Task<string> Update(Class model)
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("UpdateClass", new {model.ClassID, model.ClassName, model.GradeLevel, model.Timings, model.MaxClassSize, model.ProgramID });
                message = "Class updated successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }

            return message;
        }
    }
}
