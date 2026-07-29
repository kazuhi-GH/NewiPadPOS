using System.Web.Mvc;
using Net48MvcSample.Models;

namespace Net48MvcSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new MessageViewModel
            {
                Title = ".NET Framework 4.8 MVC Sample",
                Description = "C#で作成した最小構成のASP.NET MVC 5サンプルです。"
            };

            return View(model);
        }
    }
}
