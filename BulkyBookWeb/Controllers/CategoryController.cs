using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace BulkyBookWeb.Controllers
{
    [Authorize (Roles = SD.Role_Admin )]
    public class CategoryController : Controller
    {
        //private readonly AppDbContext db;

        //private readonly ICategoryRepository db;

        //  Now use the UnitOfWork  General  handling of All Repositories
        private readonly IUnitOfWork db;

        //public CategoryController(AppDbContext db)
        //public CategoryController(ICategoryRepository db)

        //  Now use the UnitOfWork  General  handling of All Repositories
        public CategoryController(IUnitOfWork db)
        {
            this.db = db;
        }

        [BindProperty]   //   This attribute  says,   get me the value FROM the  UI page Create.cshtml VIEW  @model Category
        public Category obj { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        List<Category> categories = new List<Category>();

        public IActionResult Index()
        {
            //   here you do NOT need the ToList() because you are assigning to an EXISTING defined LIST
            //IEnumerable<Category> objCategoryList = this.db.Categories;

            //   need the ToList()  because assigning to a  field
            //categories = this.db.Categories.ToList();

            //   here you do NOT need the ToList() because you are assigning to a Property
            //Categories = this.db.Categories;
            //Categories = this.db.GetAll();

            //  Now use the UnitOfWork  General  handling of All Repositories
            Categories = this.db.Category.GetAll();

            //http://www.binaryintellect.net/articles/8e64d05b-ab2e-45f6-b7f5-b8a90168915e.aspx
            //string url = string.Format("/home/index?name={0}",
            //Categories.Select(c => c.Name).FirstOrDefault());
            //return Redirect(url);

            //TempData["message"] = "Hello Peyton";
            //return RedirectToAction("Index", "Home");

            return View(Categories);

            //return View(categories);
            //return View(objCategoryList);
        }
        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString()) 
            {
                ModelState.AddModelError("Custom Error", "The Display Order cannot exactly match the Name");
                //OR  you could put  "Name"  or  "DisplayOrder"  as the First argument, to show the message for that Property

                TempData["success"] = string.Empty;
                TempData["error"] = "The Name and Display Order are the same - which is not allowed"; /* you do not need this line, Errors are handled from above*/
            }
            if (ModelState.IsValid)
            {
                //this.db.Categories.Add(obj);
                //this.db.SaveChanges();
                //    or  this way using the CategoryRepository
                //this.db.Add(obj);
                //this.db.Save();

                //  Now use the UnitOfWork  General  handling of All Repositories
                this.db.Category.Add(obj);
                this.db.Save();
                TempData["success"] = "Category was successfully Created";
                TempData["error"] = string.Empty;
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //Get
        public IActionResult Edit(int? id)
        {
            if (id==null||id == 0)
            {
                return NotFound();
            }

            //Category CategoryFromDb  = this.db.Categories.FirstOrDefault(c => c.Id == id);  //  this uses the AppDbContext 
            //Category CategoryFromDb  = this.db.GetFirstOrDefault(c => c.Id == id);   //  this uses the ICategoryRepository
            //CategoryFromDb =  this.db.Categories.SingleOrDefault(c => c.Id == id);
            //Category? CategoryFromDb = this.db.Categories.Find(id);   //  will work if  'id'  is a Primary Key
            //Category? CategoryFromDb = this.db.Categories.Where(x=> x.Id==id).FirstOrDefault();

            //  Now use the UnitOfWork  General  handling of All Repositories
            Category? CategoryFromDb = this.db.Category.GetFirstOrDefault(x => x.Id == id);  //  this uses the IUnitOfWork 

            if (CategoryFromDb == null || id == 0)
            {
                return NotFound();
            }

            return View(CategoryFromDb);
        }


        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)    //  here the  Category Class is used "directly"  for the information comming back from the Category EDIT View
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Custom Error", "The Display Order cannot exactly match the Name");
                //OR  you could put  "Name"  or  "DisplayOrder"  as the First argument, to show the message for that Property

                TempData["success"] = string.Empty;
                TempData["error"] = "The Name and Display Order are the same - which is not allowed"; /* you do not need this line, Errors are handled from above*/
            }
            if (ModelState.IsValid)
            {
                //this.db.Categories.Update(obj);  this uses the  AppDbContext
                //this.db.SaveChanges();

                //this.db.Update(obj);   // this uses the ICategoryRepository
                //this.db.Save();

                //  Now use the UnitOfWork  General  handling of All Repositories
                this.db.Category.Update(obj);     //  Here  the  Update to Category table within the DB, is Actually being done INSIDE the CategoryRepository
                                                  //   "obj"  has  ALL the Category CLASS   information
                                                  //  but handled in TWO STEPS  with Product table
                this.db.Save();
                TempData["success"] = "Category was successfully Updated";
                TempData["error"] = string.Empty;
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //Category CategoryFromDb  = this.db.Categories.FirstOrDefault(c => c.Id == id);    //  this uses the AppDbContext
            //CategoryFromDb =  this.db.Categories.SingleOrDefault(c => c.Id == id);
            //Category? CategoryFromDb = this.db.Categories.Find(id);

            //Category CategoryFromDb = this.db.GetFirstOrDefault(c => c.Id == id);     //  this uses the ICategoryRepository

            //  Now use the UnitOfWork  General  handling of All Repositories
            Category CategoryFromDb = this.db.Category.GetFirstOrDefault(c => c.Id == id);    //  this uses the IUnitOfWork 

            if (CategoryFromDb == null || id == 0)
            {
                return NotFound();
            }
            return View(CategoryFromDb);
        }



        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category obj)
        {
            //this.db.Categories.Update(obj);   //  this uses the  AppDbContext
            //this.db.SaveChanges();

            //this.db.Remove(obj);   //  this uses the  ICategoryRepository
            //this.db.Save();

            //  Now use the UnitOfWork  General  handling of All Repositories
            this.db.Category.Remove(obj);
            this.db.Save();
            TempData["success"] = "Category was successfully Deleted";
            TempData["error"] = string.Empty;
            return RedirectToAction("Index");
        }
    }
}
