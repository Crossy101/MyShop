using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cache["products"] as List<Product>;

            if(products == null)
            {
                products = new List<Product>();
            }
        }

        public void Commit()
        {
            cache["products"] = products;
        }

        public void Insert(Product product)
        {
            products.Add(product);
        }

        public void Update(Product product)
        {
            Product productToUpdate = products.Find(p => p.Id == product.Id);

            if (productToUpdate == null)
                throw new Exception("Product not found!");

            productToUpdate = product;
        }

        public Product Find(string Id)
        {
            Product productToFind = products.Find(p => p.Id == Id);

            if (productToFind == null)
                throw new Exception("Product cannot be found!");

            return productToFind;
        }

        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(string Id)
        {
            Product productToDelete = products.Find(product => product.Id == Id);

            if (productToDelete == null)
                throw new Exception("Product cannot be found!");

            products.Remove(productToDelete);
        }

    }
}
