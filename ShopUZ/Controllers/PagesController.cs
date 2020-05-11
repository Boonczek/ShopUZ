using ShopUZ.Models.Data;
using ShopUZ.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopUZ.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index/{pages}
        public ActionResult Index(string page = "")
        {

            //ustaiwamy adres naszej strony
            if (page == "")
                page = "home";

            //deklarujemy PageVM i PageDTO
            PageVM model;
            PageDTO dto;
            //sprawdzamy czy strona istnieje
            using(Db db = new Db())
            {
                if(!db.Pages.Any(x => x.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new {page = "" });
                }
            }

            //pobieramy PageDTO
            using(Db db = new Db())
            {
                dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }

            //ustawiamy tytul naszej strony
            ViewBag.PageTitle = dto.Title;

            //sprawdzamy czy strona ma pasek boczny
            if(dto.HasSidebar == true)
            {
                ViewBag.Sidebar = "Tak";
            }

            else
            {
                ViewBag.Sidebar = "Nie";
            }

            //inicializacja pageVM
            model = new PageVM(dto);

            //zwracamy widok z pageVM


            return View(model);
        }

        public ActionResult PagesMenuPartial()
        {
            //deklaracja PageVM
            List<PageVM> pageVMList;

            //pobranie stron
            using(Db db = new Db())
            {
                pageVMList = db.Pages.ToArray()
                    .OrderBy(x => x.Sorting)
                    .Where(x => x.Slug != "home")
                    .Select(x => new PageVM(x))
                    .ToList();
            }
            //zwracamy PageVMList
            return PartialView(pageVMList);
        }
    }
}