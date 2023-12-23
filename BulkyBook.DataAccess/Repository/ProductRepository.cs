using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext db;

        public ProductRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }


        //public void Save()
        //{
        //   this.db.SaveChanges();
        //}

        public void Update(Product obj)
        {
            //   Instead of doing a complete UPDATE  in  JUST  ONE   line below,
            //   you can also   update one-by-one if  you ONLY need to Update just ONE or TWO columns ?
            //this.db.Products.Update(obj);

            // Then do this way instead --->
            var objFromDb = this.db.Products.FirstOrDefault(x=> x.Id == obj.Id);    
            if (objFromDb != null) {
                objFromDb.ISBN = obj.ISBN;
                objFromDb.ListPrice= obj.ListPrice;
                //objFromDb.Price = obj.Price;
                //objFromDb.Price100 = obj.Price100;
                //objFromDb.Price50 = obj.Price50;
                objFromDb.Author = obj.Author;
                objFromDb.Description = obj.Description;
                objFromDb.Title= obj.Title;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.CoverTypeId = obj.CoverTypeId;

                if (objFromDb.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }

                //  The Actual  UPDATE   was done  in the above code ,   ONE  column at a time. 
            }
        }
    }
}
