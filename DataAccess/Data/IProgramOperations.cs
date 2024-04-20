using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public interface IProgramOperations
    {
        Task<string> Add(Program model);

        Task<string> Update(Program model);

        Task<string> Delete(int ProgramId);

        Task<Program> GetProgram(int ProgramID);

        Task<List<Program>> List();

        Task<List<Program>> ActiveProgramsList();
    }
}
