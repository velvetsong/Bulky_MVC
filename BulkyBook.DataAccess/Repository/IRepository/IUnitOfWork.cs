using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        //  Define the Interface ReadOnly properties for each Class Repository

        public ICategoryRepository Category { get; }

        public ICoverTypeRepository CoverType { get; }

        public IProductRepository Product { get;  }

		public ICompanyRepository Company { get; }

		void Save();
    }
}
