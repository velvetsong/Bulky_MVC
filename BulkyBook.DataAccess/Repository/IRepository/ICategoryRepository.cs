using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);

        //IEnumerable<Category> GetCategoryListForDropDown = this.db.Categories; 

        IEnumerable<SelectListItem> GetCategoryListForDropDown();

        void Save();
    }
}
