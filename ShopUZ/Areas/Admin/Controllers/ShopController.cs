using ShopUZ.Models.Data;
using ShopUZ.Models.ViewModels.Shop;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShopUZ.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()

        {
            // deklaracja listy kategorii do wyświetlenia
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                categoryVMList = db.Categories
                .ToArray()
                .OrderBy(x => x.Sorting)
                .Select(x => new CategoryVM(x)).ToList();
            }

            return View(categoryVMList);
        }


        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            // Deklaracja id
            string id;

            using (Db db = new Db())
            {
                // sprawdzenie czy nazwa kategorii jest unikalna
                if (db.Categories.Any(x => x.Name == catName))
                    return "tytulzajety";

                // Inicjalizacja DTO
                CategoryDTO dto = new CategoryDTO
                {
                    Name = catName,
                    Slug = catName.Replace(" ", "-").ToLower(),
                    Sorting = 1000
                };

                // zapis do bazy
                db.Categories.Add(dto);
                db.SaveChanges();

                // pobieramy id
                id = dto.Id.ToString();
            }

            return id;
        }

    }
    
}