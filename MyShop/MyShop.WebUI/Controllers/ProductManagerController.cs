using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
using MyShop.Core.Contracts;
using System.IO;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {

        IRepository<Product> productContext;
        IRepository<ProductCategory> productCategoryContext;

        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            this.productContext = productContext;
            this.productCategoryContext = productCategoryContext;
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = productContext.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            viewModel.product = new Product();
            viewModel.productCategories = productCategoryContext.Collection();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if(!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }

                productContext.Insert(product);
                productContext.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = productContext.Find(Id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.product = product;
                viewModel.productCategories = productCategoryContext.Collection();
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
        {
            Product productToEdit = productContext.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if(!ModelState.IsValid)
                {
                    ProductManagerViewModel viewModel = new ProductManagerViewModel();
                    viewModel.product = product;
                    viewModel.productCategories = productCategoryContext.Collection();
                    return View(viewModel);
                }

                if (file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }

                productToEdit.Name = product.Name;
                productToEdit.Category = product.Category;
                productToEdit.Price = product.Price;
                productToEdit.Description = product.Description;

                productContext.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Product productToDelete = productContext.Find(Id);

            if(productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = productContext.Find(Id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                productContext.Delete(Id);
                productContext.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}