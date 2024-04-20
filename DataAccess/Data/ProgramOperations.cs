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
    public class ProgramOperations : IProgramOperations
    {
        private readonly ISqlDataAccess _dbo;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private ILogger<ProgramOperations> _logger;

        public ProgramOperations(ISqlDataAccess dbo, IConfiguration config, IMemoryCache cache, ILogger<ProgramOperations> logger)
        {
            _dbo = dbo;
            _config = config;
            _cache = cache;
            _logger = logger;
        }
        public async Task<string> Add(Program model)
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("InsertProgram", new { model.ProgramName, model.Description, model.StartDate });
                message = "Program saved successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }

            return message;
        }

        public async Task<Program> GetProgram(int ProgramID)
        {
            Program data = new Program();
            try
            {
                var resp = await _dbo.LoadData<Program,dynamic>("SelectProgram", new { ProgramID });
                data = resp.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return data;
        }

        public async Task<string> Delete(int programId)
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("DeleteProgram", new { ProgramId = programId });
                message = "Program and all dependent data has been deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }

            return message;
        }

        public async Task<List<Program>> List()
        {
            List<Program>? list = new();
            try
            {
                IEnumerable<Program> progamList = await _dbo.LoadData<Program, dynamic>("SelectPrograms", new { });
                list = progamList.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return list;
        }

        public async Task<List<Program>> ActiveProgramsList()
        {
            List<Program>? list = new();
            try
            {

                IEnumerable<Program> progamList = await _dbo.LoadData<Program, dynamic>("GetActivePrograms", new { });
                list = progamList.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return list;
        }

        public async Task<string> Update(Program model)
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("UpdateProgram", new {model.ProgramID, model.ProgramName, model.Description, model.StartDate });
                message = "Program updated successfully";
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
