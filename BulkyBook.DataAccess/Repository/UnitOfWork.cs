using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork  : IUnitOfWork 
    {
        private readonly AppDbContext db;

        public UnitOfWork(AppDbContext db) 
        {
            this.db = db;
            Category = new CategoryRepository(this.db);   /*this will give the CategoryRepository the ability to do the UPDATE*/
            CoverType = new  CoverTypeRepository(this.db);  /*this will cause the CoverTypeRepository the ability to do the UPDATE*/
            Product = new ProductRepository(this.db);   /*this will cause the ProductRepository the ability to do the UPDATE*/
			Company = new CompanyRepository(this.db);   /*this will cause the CompanyRepository the ability to do the UPDATE*/
		}

        public ICategoryRepository Category { get; private set; }

        public ICoverTypeRepository CoverType { get; private set; }

        public IProductRepository  Product { get; private set; }

		public ICompanyRepository Company { get; private set; }

		public void Save()
        {
            this.db.SaveChanges();    //   this will Save  ALL changes , iserts, deletes, updates to  ALL TABLES  at the SAME time
                                      //  this Coordinates "persistant" CHANGES across MULTIPLE Repositories in a SINGLE Transaction
        }
    }
}
