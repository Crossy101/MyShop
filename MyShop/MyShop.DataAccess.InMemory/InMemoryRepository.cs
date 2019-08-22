using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;

            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T item)
        {
            items.Add(item);
        }

        public void Update(T item)
        {
            T itemToUpdate = items.Find(p => p.Id == item.Id);

            if (itemToUpdate == null)
            {
                throw new Exception("ProductCategory not found!");
            }
            else
            {
                itemToUpdate = item;
            }
        }

        public T Find(string Id)
        {
            T itemToFind = items.Find(p => p.Id == Id);

            if (itemToFind == null)
            {
                throw new Exception("ProductCategory not found!");
            }
            else
            {
                return itemToFind;
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T itemToDelete = items.Find(item => item.Id == Id);

            if (itemToDelete == null)
            {
                throw new Exception("ProductCategory cannot be found!");
            }
            else
            {
                items.Remove(itemToDelete);
            }
        }
    }
}
