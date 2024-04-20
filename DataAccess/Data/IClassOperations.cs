using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public interface IClassOperations
    {
        Task<string> Add(Class model);

        Task<string> Update(Class model);

        Task<string> Delete(int ClassId);
        Task<Class> Get(int ClassId);

        Task<List<Class>> FilteredList(int ProgramId);
        Task<List<Class>> List();
    }
}
