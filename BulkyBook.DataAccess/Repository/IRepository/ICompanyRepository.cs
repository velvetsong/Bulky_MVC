using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {

        IEnumerable<SelectListItem> GetCompanyListForDropDown();


        void Update(Company obj);
       //void Save();
    }
}
