using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyBookWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork db;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork  db )
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            //Category? data = new Category();
            //data.Name = Request.QueryString["name"];
            //return View(data);

            IEnumerable<Product> products = this.db.Product.GetAll(includeNavigationProperties: "Category,CoverType");

            return View(products);
        }

		public IActionResult Details(int id)
        {
            //Category? data = new Category();
            //data.Name = Request.QueryString["name"];
            //return View(data);

            ShoppingCartVM cartObj = new()
            {
                Count = 1,
                Product = this.db.Product.GetFirstOrDefault(x => x.Id == id, includeNavigationProperties: "Category,CoverType")
            };
            return View(cartObj);
	    }


		public IActionResult Privacy()
        {
            return View();
        }        
    }
}