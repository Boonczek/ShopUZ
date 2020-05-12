using ShopUZ.Models.Data;
using ShopUZ.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopUZ.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            // deklarujemy CategoryVM list
            List<CategoryVM> categoryVMList;

            // inicjalizacja listy
            using (Db db = new Db())
            {
                categoryVMList = db.Categories
                                   .ToArray()
                                   .OrderBy(x => x.Sorting)
                                   .Select(x => new CategoryVM(x))
                                   .ToList();
            }

            // zwracamy partial z lista
            return PartialView(categoryVMList);
        }

        //GET: /shop/Category/name
        public ActionResult Category(string name)
        {
            //delkaracja ProductVMList
            List<ProductVM> productVMLIst;

            using(Db db = new Db())
            {
                //pobranie id kategorii
                CategoryDTO categoryDTO = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDTO.Id;

                //Inicializacja listy produktów
                productVMLIst = db.Products
                    .ToArray()
                    .Where(x => x.CategoryId == catId)
                    .Select(x => new ProductVM(x)).ToList();

                //pobieramy nazwe kategorii
                var productCat = db.Products.Where(x => x.CategoryId == catId).FirstOrDefault();
                ViewBag.CategoryName = productCat.CategoryName;
            }
            //zwracamy widok z lista produktow
            return View(productVMLIst);
        }

        //GET: /shop/product-szczegoly/name
        [ActionName("product-szczegoly")]
        public ActionResult ProductDetails(string name)
        {
            //deklaracja productVM i productDTO
            ProductVM model;
            ProductDTO dto;

            //inicializacja productId
            int id = 0;

            using(Db db = new Db())
            {
                //sprawdzamy czy produkt istnieje
                if (!db.Products.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                //inicializacja productDTO
                dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();

                //pobranie id
                id = dto.Id;

                //inicializacja modelu
                model = new ProductVM(dto);
            }

            //pobieramy galerie zdjec dla wybranego produktu
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                           .Select(fn => Path.GetFileName(fn));

            //zwracamy widok z modelem
            return View("ProductDetails", model);
        }

    }
}