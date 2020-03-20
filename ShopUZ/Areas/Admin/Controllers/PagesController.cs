using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShopUZ.Models.Data;
using ShopUZ.Models.ViewModels.Pages;

namespace ShopUZ.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Deklaracja listy PageViewModel
            List<PageVM> pagesList;
            
            using (Db db = new Db())
            {
                //Inicializacja listy
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            //Zwracamy strony do widoku


            return View(pagesList);
        }
    }
}