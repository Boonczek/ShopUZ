using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShopUZ.Models.Data;
using ShopUZ.Models.ViewModels.Pages;

namespace ShopUZ.Areas.Admin.Controllers
{

    [Authorize(Roles = "Admin")]
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        [HttpGet]
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

        // POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //pobranie Id strony
                int id = model.Id;

                //inicializacja slug
                string slug = "home";

                //pobranie strony do edycji
                PageDTO dto = db.Pages.Find(id);

                if(model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }

                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower(); 
                    }
                }
                //Sprawdzamy unikalność strony, adresu
                if(db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || 
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Strona lub adres strony już istnieje!");
                }
                //Modyfikacje DTO
                dto.Title = model.Title;
                dto.Slug = slug;
                dto.HasSidebar = model.HasSidebar;
                dto.Body = model.Body;

                //zapis edytowanej strony do bazy
                db.SaveChanges();
            }
            //ustawienie komunikatu TempData
            TempData["SM"] = "Wyedytowałeś stronę";

            //Redirect
            return RedirectToAction("EditPage");
        }

        // GET: Admin/Pages/Details/id
        [HttpGet]
        public ActionResult Details(int id)
        {
            //deklaracja PageVM
            PageVM model;

            using (Db db = new Db())
            {
                //pobranie strony o id
                PageDTO dto = db.Pages.Find(id);

                //Sprawdzamy czy strona o takim id istnieje
                if (dto == null)
                {
                    return Content("Strona o podanym id nie istnieje!");
                }

                //Inicializacja PageVM
                model = new PageVM(dto);


            }
            return View(model);
        }

        // GET: Admin/Pages/Delete/id
        [HttpGet]
        public ActionResult Delete(int id)
        {

            using(Db db = new Db())
            {
                //pobranie strony do usunięcia
                PageDTO dto = db.Pages.Find(id);

                //usuwanie wybranej strony z bazy danych
                db.Pages.Remove(dto);

                //zapis zmian
                db.SaveChanges();
            }
            //przekierowanie Redirect
            return RedirectToAction("Index");
        }

        // POST: Admin/Pages/ReorderPages
        [HttpPost]
        public ActionResult ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;
                PageDTO dto;

                //sortowanie stron, zapis w bazie danych
                foreach(var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();
                    count++;
                }

            }

            return View();
        }

        //GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            //Deklaracja SidebarVM
            SidebarVM model;

            using (Db db = new Db())
            {
                //Pobieramy SidebarDTO
                SidebarDTO dto = db.Sidebar.Find(1);

                //Inicializacja modelu
                model = new SidebarVM(dto);

            }

            return View(model);
        }

        //POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using(Db db = new Db())
            {
                //Pobieramy Sidebar DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                //Modyfikacja Sidebar
                dto.Body = model.Body;

                //Zapis w bazie danych
                db.SaveChanges();
            }

            //Ustawiwamy komunikat o modyfikacji Sidebar
            TempData["SM"] = "Zmodyfikowałeś Pasek Boczny";
            //Przekierowanie Redirect
            return RedirectToAction("EditSidebar");
        }
    }

}