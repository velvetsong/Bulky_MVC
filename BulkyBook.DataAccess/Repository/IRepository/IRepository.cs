using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //  so below were are just adding the STUB definitions   

        //   previously in the Category Controller   --->
        //  for the INDEX ---->  public IEnumerable<Category> Categories { get; set; }
        // for the CREATE --->   this.db.Categories.Add(obj);
        // for the EDIT -->   Category? CategoryFromDb = this.db.Categories.Where(x=> x.Id==id).FirstOrDefault(); 
        // for the DELETE --->  this.db.Categories.Remove(obj);

        //  new code ---->
        public IEnumerable<T>  GetAll(string? includeNavigationProperties = null);    //  here  T   can be ANY CLASS

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);

        T GetFirstOrDefault(Expression<Func<T, bool>> predicate, string?  includeNavigationProperties = null);
    }
}
