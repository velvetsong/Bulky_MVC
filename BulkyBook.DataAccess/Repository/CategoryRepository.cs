using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository 
    {
        private readonly AppDbContext db;

        public CategoryRepository(AppDbContext db)    : base(db)
        {
            this.db = db;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }


        public IEnumerable<SelectListItem> GetCategoryListForDropDown()
        {
            return this.db.Categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
        }

        public void Update(Category obj)
        {
            //  so  here you are Actually  Updating the  LIST  of Categories,  just like you did within the Category  CONTROLLER
            this.db.Categories.Update(obj);   //  and this  "obj"  has ALL the  Category VALUES from the Category EDIT VIEW
                                              //  because the  EDIT View  and  the  Category CLASS  are  "one in the same"  because
                                              //    the EDIT VIEW   has  @model Category
        }
    }
}
