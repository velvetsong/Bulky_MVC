using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class ProductVM
    {

        //    having the Product Class defined as a whole Property ITSELF, 
        // this way when passing  property values back and forth between View and ViewModel, these values will only be stored in the Product Class,
        // and NOT "individual" properties within THIS CLASS,   thus no confusion on 'where' the true values are stored 

        public Product Product { get; set; }


        public ProductVM()
        {
            Product = new Product();
        }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }


    }
}
