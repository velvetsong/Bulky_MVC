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
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly AppDbContext db;

        public CoverTypeRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }


        public IEnumerable<SelectListItem> GetCoverTypeListForDropDown()
        {
            return this.db.CoverTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
        }

        //public void Save()
        //{
        //   this.db.SaveChanges();
        //}

        public void Update(CoverType obj)
        {
            //  so  here you are  Updating the  LIST  of CoverTypes,  just like you did within the CoverType  CONTROLLER
            this.db.CoverTypes.Update(obj);
        }
    }
}
