using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class LoadHtmlController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // ooooooooooooooooooooooooooo  just for testing below    oooooooooooooooooooooooooooo
        //public ActionResult MyHtml()
        //{
        //    var result = new FilePathResult("~/Views/HtmlPage1.html", "text/html");
        //    return result;
        //}

        //public ActionResult MyHtml(string htmlPageName)
        //{
        //    var result = new FilePathResult($"~/Views/{htmlPageName}.html", "text/html");
        //    return result;
        //}

        //[ChildActionOnly]
        //public ActionResult GetHtmlPage(string path)
        //{
        //    return new FilePathResult(path, "text/html");
        //}
    // ooooooooooooooooooooooooooo  just for testing above   oooooooooooooooooooooooooooo
}
}
