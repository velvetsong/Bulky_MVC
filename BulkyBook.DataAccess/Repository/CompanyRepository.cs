using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly AppDbContext db;

        public CompanyRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }

        public IEnumerable<SelectListItem> GetCompanyListForDropDown()
        {
            return this.db.Companies.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
        }

        //public void Save()
        //{
        //   this.db.SaveChanges();
        //}



        public void Update(Company obj)
        {
            //  so  here you are  Updating the  LIST  of Companyies,  just like you did within the Company CONTROLLER
            this.db.Companies.Update(obj);
        }
    }
}
