using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BulkyBookWeb.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        //private readonly AppDbContext db;
        //  Now use the UnitOfWork  General  handling of All Repositories
        //private readonly ICoverTypeRepository  db;
        private readonly IUnitOfWork db;

        //public CoverTypeController(ICoverTypeRepository db)
        //  Now use the UnitOfWork  General  handling of All Repositories
        public  CoverTypeController(IUnitOfWork db)
        {
            this.db = db;
        }

        [BindProperty]   //   This attribute  says,   get me the value FROM the  UI page Create.cshtml VIEW  @model CoverType
        public CoverType obj { get; set; }

        public IEnumerable<CoverType> CoverTypes { get; set; }
        List<CoverType>  covertypes = new List<CoverType>();

        public IActionResult Index()
        {
            //   here you do NOT need the ToList() because you are assigning to an EXISTING defined LIST
            //IEnumerable<CoverType> objCoverTypeList = this.db.CoverTypes;

            //   need the ToList()  because assigning to a  field
            //covertypes = this.db.CoverTypes.ToList();

            //   here you do NOT need the ToList() because you are assigning to a Property
            //CoverTypes = this.db.CoverTypes;
            //CoverTypes = this.db.GetAll();

            //  Now use the UnitOfWork  General  handling of All Repositories
            CoverTypes = this.db.CoverType.GetAll();

            return View(CoverTypes);
            //return View(CoverTypes);
            //return View(objCoverTypeList);
        }
        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                //this.db.CoverTypes.Add(obj); 
                //this.db.SaveChanges();
                //this.db.Add(obj);

                //  Now use the UnitOfWork  General  handling of All Repositories
                this.db.CoverType.Add(obj);
                this.db.Save();
                TempData["success"] = "CoverType was successfully Created";
                TempData["error"] = string.Empty;
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //CoverType CoverTypeFromDb  = this.db.CoverTypes.FirstOrDefault(c => c.Id == id);   
            //CoverTypeFromDb =  this.db.CoverTypes.SingleOrDefault(c => c.Id == id);
            //CoverType? CoverTypeFromDb = this.db.CoverTypes.Find(id);   //  will work if  'id'  is a Primary Key
            //CoverType? CoverTypeFromDb = this.db.CoverTypes.Where(x=> x.Id==id).FirstOrDefault();

            //  Now use the UnitOfWork  General  handling of All Repositories
            CoverType? CoverTypeFromDb = this.db.CoverType.GetFirstOrDefault(x => x.Id == id);

            if (CoverTypeFromDb == null || id == 0)
            {
                return NotFound();
            }

            return View(CoverTypeFromDb);
        }


        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                //this.db.CoverTypes.Update(obj);
                //this.db.SaveChanges();
                //this.db.Update(obj);

                //  Now use the UnitOfWork  General  handling of All Repositories
                this.db.CoverType.Update(obj);
                this.db.Save();
                TempData["success"] = "CoverType was successfully Updated";
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

            //CoverType CoverTypeFromDb  = this.db.CoverTypes.FirstOrDefault(c => c.Id == id);   
            //CoverTypeFromDb =  this.db.CoverTypes.SingleOrDefault(c => c.Id == id);
            //CoverType? CoverTypeFromDb = this.db.CoverTypes.Find(id);

            //CoverType CoverTypeFromDb = this.db.GetFirstOrDefault(c => c.Id == id);

            //  Now use the UnitOfWork  General  handling of All Repositories
            CoverType CoverTypeFromDb = this.db.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (CoverTypeFromDb == null || id == 0)
            {
                return NotFound();
            }
            return View(CoverTypeFromDb);
        }



        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(CoverType obj)
        {
            //this.db.CoverTypes.Update(obj);
            //this.db.SaveChanges();
            //this.db.Remove(obj);

            //  Now use the UnitOfWork  General  handling of All Repositories
            this.db.CoverType.Remove(obj);
            this.db.Save();
            TempData["success"] = "CoverType was successfully Deleted";
            TempData["error"] = string.Empty;
            return RedirectToAction("Index");
        }
    }
}
