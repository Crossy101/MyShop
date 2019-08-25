using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> productContext;
        IRepository<ProductCategory> productCategoryContext;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            this.productContext = productContext;
            this.productCategoryContext = productCategoryContext;
        }

        public ActionResult Index(string Category = null)
        {
            List<Product> products;
            List<ProductCategory> productCategories = productCategoryContext.Collection().ToList();

            if(Category == null)
            {
                products = productContext.Collection().ToList();
            }
            else
            {
                products = productContext.Collection().Where(p => p.Category == Category).ToList();
            }

            ProductListViewModel productListViewModel = new ProductListViewModel();
            productListViewModel.products = products;
            productListViewModel.productCategories = productCategories;

            return View(productListViewModel);
        }

        public ActionResult Details(string Id)
        {
            Product product = productContext.Find(Id);
            if(product != null)
            {
                return View(product);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}