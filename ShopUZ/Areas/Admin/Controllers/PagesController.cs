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

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {

            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Sprawdzanie Model state stanu formularza
            if (!ModelState.IsValid)
            {
                return View(model);
            } 

            using (Db db = new Db())
            {
                string slug;
                //inicializacja PageDTO
                PageDTO dto = new PageDTO();
               
                //Gdy nie mamy adresu strony to przypisujemy tytu
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();    
                }

                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Zapobieagamy dodania takiej samej nazwy strony to przypisujemy tytuł 
                if(db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Ten tytuł lub adres strony już istnieje.");
                        return View(model);
                }
                dto.Title = model.Title;
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 1000;

                // Zapis dto
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            TempData["SM"] = "Dodałeś nową stronę!";
            return RedirectToAction("AddPage");
        }
        // GET: Admin/Pages/EditPage
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Deklaracja PageVM
            PageVM model;

            using (Db db = new Db())
            {
                //Pobieramy strone z bazy o przekazanym id
                PageDTO dto = db.Pages.Find(id);
                //Sprawdzamy czy taka strona istnieje
                if (dto == null)
                {
                    return Content("Strona nie isniteje!");
                }
                model = new PageVM(dto);
            }

            return View(model);
        }
    }

}