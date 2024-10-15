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
    public class CompanyController : Controller
    {
        //private readonly AppDbContext db;
        //  Now use the UnitOfWork  General  handling of All Repositories
        //private readonly ICompanyRepository  db;
        private readonly IUnitOfWork db;

        //public CompanyController(ICompanyRepository db)
        //  Now use the UnitOfWork  General  handling of All Repositories
        public CompanyController(IUnitOfWork db)
        {
            this.db = db;
        }

        //[BindProperty]   //   This attribute  says,   get me the value FROM the  UI page Create.cshtml VIEW  @model Company
        //public Company obj { get; set; }   // if you are using  CompanyVM view model , then you do not need this line to store data

        //public IEnumerable<Company> Companies { get; set; }
        //List<Company> Companies = new List<Company>();

        [HttpGet]
        public IActionResult Index()
        {
            //   here you do NOT need the ToList() because you are assigning to an EXISTING defined LIST
            //IEnumerable<Company> objCompanyList = this.db.Companies;

            //   need the ToList()  because assigning to a field
            //Companies = this.db.Companies.ToList();

            //   here you do NOT need the ToList() because you are assigning to a Property
            //Companies = this.db.Companies;
            //Companies = this.db.GetAll();

            //  Now use the UnitOfWork  General  handling of All Repositories
            //   here you do NOT need the ToList() because you are assigning to an EXISTING defined LIST
            //Companies = this.db.Company.GetAll();
            //return View(Companies);

            return View();
            //return View(objCompanyList);
        }

        //Get
        public IActionResult CreateUpdate(int? id)
        {
            Company company = new Company();

            if (id == null || id == 0)
            {
                // Create New Company
            }
            else
            {
                company = this.db.Company.GetFirstOrDefault(u => u.Id == id);
                // Update Company
                // 
            }
            return View(company);   //  the Company  Index View  is  "Tightly"  Bound  to the Company Class
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult CreateUpdate(Company obj)
        public IActionResult CreateUpdate(Company obj)   // coming back, the Company Class has its properties populated
        {
            if (ModelState.IsValid)
            {

                //this.db.Companies.Update(obj);
                //this.db.SaveChanges();
                //this.db.Update(obj);

                //  Now use the UnitOfWork  General  handling of All Repositories

                if (obj.Id == 0)
                {
                    //  Notice within this Action Method, there was NO ACTUAL Formatting to Populate the individual Properties of the Company CLASS
                    this.db.Company.Add(obj); // remember, the CreateUpdate VIEW  did all the work of POPULATING the Properties of the Company CLASS 
                    TempData["success"] = string.Empty;
                    TempData["success"] = "Company was successfully Created";
                    TempData["error"] = string.Empty;
                }
                else
                {
                    this.db.Company.Update(obj);
                    TempData["success"] = string.Empty;
                    TempData["success"] = "Company was successfully Updated";
                    TempData["error"] = string.Empty;
                }

                this.db.Save();

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

        //    //Company CompanyFromDb  = this.db.companies.FirstOrDefault(c => c.Id == id);   
        //    //CompanyFromDb =  this.db.companies.SingleOrDefault(c => c.Id == id);
        //    //Company? CompanyFromDb = this.db.companies.Find(id);

        //    //Company CompanyFromDb = this.db.GetFirstOrDefault(c => c.Id == id);

        //    //  Now use the UnitOfWork  General  handling of All Repositories
        //    Company CompanyFromDb = this.db.Company.GetFirstOrDefault(c => c.Id == id);

        //    if (CompanyFromDb == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    return View(CompanyFromDb);
        //}


        ////Post
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete(Company obj)
        //{
        //    //this.db.companies.Update(obj);
        //    //this.db.SaveChanges();
        //    //this.db.Remove(obj);

        //    //  Now use the UnitOfWork  General  handling of All Repositories
        //    this.db.Company.Remove(obj);
        //    this.db.Save();
        //    TempData["success"] = "Company was successfully Deleted";
        //    TempData["error"] = string.Empty;
        //    return RedirectToAction("Index");
        //}

        #region  API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = this.db.Company.GetAll();

            //IEnumerable<Company> companyList = this.db.Company.GetAll();

            return Json(new { data = companyList });
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            //  Now use the UnitOfWork  General  handling of All Repositories
            var obj = this.db.Company.GetFirstOrDefault(c => c.Id == id);
           

            if (obj == null || id == 0)
            {
                return Json(new { success=false, message="Error while deleting" });
            }


            this.db.Company.Remove(obj);
            this.db.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
