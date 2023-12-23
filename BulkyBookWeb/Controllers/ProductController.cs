using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Packaging.Signing;
using System.Data;
using System.Drawing;

namespace BulkyBookWeb.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        //private readonly AppDbContext db;
        //  Now use the UnitOfWork  General  handling of All Repositories
        //private readonly IProductRepository  db;
        private readonly IUnitOfWork db;
        private readonly IWebHostEnvironment env;

        //public ProductController(IProductRepository db)
        //  Now use the UnitOfWork  General  handling of All Repositories
        public ProductController(IUnitOfWork db,  IWebHostEnvironment hostEnvironment )
        {
            this.db = db;
            this.env  = hostEnvironment;
        }

        //[BindProperty]   //   This attribute  says,   get me the value FROM the  UI page Create.cshtml VIEW  @model Product
        //public Product obj { get; set; }   // if you are using  ProductVM view model , then you do not need this line to store data

        public IEnumerable<Product> Products { get; set; }
        List<Product> products = new List<Product>();

        public IActionResult Index()
        {
            //   here you do NOT need the ToList() because you are assigning to an EXISTING defined LIST
            //IEnumerable<Product> objProductList = this.db.Products;

            //   need the ToList()  because assigning to a  field
            //Products = this.db.Products.ToList();

            //   here you do NOT need the ToList() because you are assigning to a Property
            //Products = this.db.Products;
            //Products = this.db.GetAll();

            //  Now use the UnitOfWork  General  handling of All Repositories
            //Products = this.db.Product.GetAll();
            //return View(Products);

            return View();
            //return View(objProductList);
        }


        //Get
        public IActionResult CreateUpdate(int? id)
        {
            //Product product = new Product();
            ProductVM productVM = new ProductVM()
            {
                //    Now we are "populating"   the  Properties within the  ProductVM  Class,   first the CategoryList and CoverTypeList are populated
                //   then coming back from the Post Action,  the VIEW  would have Populated the rest of the fields within the Product Class
               
                //Product = new Product(),    //   we do not need to define this here,  it was Instantiated in the ProductVM itself

                CategoryList = this.db.Category.GetCategoryListForDropDown(),

                //CategoryList = this.db.Category.GetAll().Select(x => new SelectListItem
                //    {
                //        Text = x.Name,             //this display name is passed to the CreateUpdate VIEW, in order for the View to KNOW which text to Display
                //        Value = x.Id.ToString()    //  then  the 'selected'  Value of the 'Id'   is  populated in the Product Class  CategoryId property
                //    }),

                CoverTypeList = this.db.CoverType.GetCoverTypeListForDropDown()

                //CoverTypeList = this.db.CoverType.GetAll().Select(x => new SelectListItem
                //{
                //    Text = x.Name,
                //    Value = x.Id.ToString()
                //})
            };

            //IEnumerable<SelectListItem> CategoryList;   // instead of using this Class Member field,  use the properties in ProductVM

            //IEnumerable<SelectListItem> CoverTypeList;  // instead of using this Class Member field,  use the properties in ProductVM

            if (id == null || id == 0)
            {
                // Create New Product   //  Display Just the DropDown information, in order for the User to Select an item
            }
            else
            {
                // Display All existing  Product information
                productVM.Product = this.db.Product.GetFirstOrDefault(u => u.Id == id);
             }
            return View(productVM);   //  the Product  "CreateUpdate"  View  is  "Tightly"  Bound  to the Product ViewModel
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult CreateUpdate(Product obj)
        public IActionResult CreateUpdate(ProductVM obj,  IFormFile? file)  // coming back,
                                                    // the Product Class has its properties populated from the Product CreateUpdate VIEW
                                                    // which means, in order for the ProductVM to have "Product" data
                                                    // the Product Class MUST be  Defined as a PROPERTY  in  ProductVM
                  
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = this.env.WebRootPath;    //  this give you access to the projects  wwwwRoot folder

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();

                    var uploads = Path.Combine(wwwRootPath, @"images\products");  // this is where you are going to GET the Image FROM

                    var extension = Path.GetExtension(file.FileName);


                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                       file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                //this.db.Products.Update(obj);
                //this.db.SaveChanges();
                //this.db.Update(obj);

                //  Now use the UnitOfWork  General  handling of All Repositories

                if (obj.Product.Id == 0)    //  We did not need to do this  test for  Category  table,
                                            //  thus in Category EDIT VIEW or Category CREATE VIEW
                                            //  no hidden fields were necessary
                {
                    //  Notice within this Action Method, there was NO ACTUAL Formatting to Populate the individual Properties of the Product CLASS
                    this.db.Product.Add(obj.Product); // remember, the CreateUpdate VIEW  did all the work of POPULATING the Properties of the Product CLASS 
                }
                else
                {
                    this.db.Product.Update(obj.Product);  //  Even though the obj.Product  NOW CONTAINS ALL the Info
                                                          //  from what was entered in the View, becuase the View formats the ProductVM Object
                                                          //  you can decide NOT to UPDATE  ALL of the fields, and just some of the columns
                }

                this.db.Save();    //  the UnitOfWork's  Save() method  call
                TempData["success"] = "Product was successfully Updated";
                TempData["error"] = string.Empty;
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        ////Get
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    //Product ProductFromDb  = this.db.Products.FirstOrDefault(c => c.Id == id);   
        //    //ProductFromDb =  this.db.Products.SingleOrDefault(c => c.Id == id);
        //    //Product? ProductFromDb = this.db.Products.Find(id);

        //    //Product ProductFromDb = this.db.GetFirstOrDefault(c => c.Id == id);

        //    //  Now use the UnitOfWork  General  handling of All Repositories
        //    Product ProductFromDb = this.db.Product.GetFirstOrDefault(c => c.Id == id);

        //    if (ProductFromDb == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    return View(ProductFromDb);
        //}


        ////Post
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete(Product obj)
        //{
        //    //this.db.Products.Update(obj);
        //    //this.db.SaveChanges();
        //    //this.db.Remove(obj);

        //    //  Now use the UnitOfWork  General  handling of All Repositories
        //    this.db.Product.Remove(obj);
        //    this.db.Save();
        //    TempData["success"] = "Product was successfully Deleted";
        //    TempData["error"] = string.Empty;
        //    return RedirectToAction("Index");
        //}

        #region  API Calls
        [HttpGet]

        public IActionResult GetAll()
        {
            IEnumerable<Product> productList = this.db.Product.GetAll(includeNavigationProperties: "Category,CoverType");    //  do NOT PUT A SPACE INSIDE THIS STRING

            return Json(new { data = productList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            //  Now use the UnitOfWork  General  handling of All Repositories
            var obj = this.db.Product.GetFirstOrDefault(c => c.Id == id);
           

            if (obj == null || id == 0)
            {
                return Json(new { success=false, message="Error while deleting" });
            }

            var oldImagePath = Path.Combine(env.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            this.db.Product.Remove(obj);
            this.db.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
