using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public  class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext db;
        internal DbSet<T> dbSet;

        // previously  within  AppDbContext.cs  --->
        //     public DbSet<Category> Categories { get; set; }

 
        public Repository(AppDbContext db)
        {
            this.db = db;

            //this.db.Products.Include(x => x.Category).Include(x=> x.CoverType);  //  cannot say this here because we are not using AppDbContext ,  we are using the Repository

            //  new code ----->
            this.dbSet = this.db.Set<T>(); // using the db variable we are BINDING Category CLASS to the Database variable dbSet
                                          //  or whatever class we pass in,  and we can now use dbSet instead of  db variable 
                          //   so by doing this, if you just used the "db" variable, you wouldn't know which CLASS it came from
                          //  so   this.dbSet   has  both the database information AND  the particular CLASS info that was passed in
        }
        public void Add(T entity)
        {
            //  previously in the Category Controller --->
            //  this.db.Categories.Add(obj); 
            // but now  see how this  new   "dbSet" variable  is bound to the "Category" table via the constructor
            this.dbSet.Add(entity); 
        }

        public IEnumerable<T> GetAll(string? includeNavigationProperties = null)
        {
            //  we might want to Query our Data from the Database before doing  IEnumerable,  so use IQueryable
            IQueryable<T> query = dbSet;  //   dbSet now contains the specific  Model Entity Type, but also now contains the database information as well

            if (includeNavigationProperties != null)
            {
                foreach (var includeProp in includeNavigationProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
            
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate, string? includeNavigationProperties = null)
        {
            IQueryable<T> query = dbSet;  //   dbSet is the actual table data
            //  must filter the data first
            query = query.Where(predicate);

            if (includeNavigationProperties != null)
            {
                foreach (var includeProp in includeNavigationProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            this.dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            this.dbSet.RemoveRange(entity);
        }
    }
}
