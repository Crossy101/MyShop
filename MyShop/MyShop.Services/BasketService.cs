using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> pContext, IRepository<Basket> bContext)
        {
            this.productContext = pContext;
            this.basketContext = bContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createdIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if(cookie != null)
            {
                string basketId = cookie.Value;
                if(!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    if(createdIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createdIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddHours(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string pId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.basketItems.FirstOrDefault(i => i.productID == pId);

            if(item == null)
            {
                item = new BasketItem()
                {
                    basketID = basket.Id,
                    productID = pId,
                    quantity = 1
                };

                basket.basketItems.Add(item);
            }
            else
            {
                item.quantity += 1;
            }
            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem itemToRemove = basket.basketItems.FirstOrDefault(i => i.Id == itemId);

            if(itemToRemove != null)
            {
                basket.basketItems.Remove(itemToRemove);
                basketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            
            if(basket != null)
            {
                var results = (from b in basket.basketItems
                              join p in productContext.Collection() on b.productID equals p.Id
                              select new BasketItemViewModel()
                              {
                                  Id = b.Id,
                                  quantity = b.quantity,
                                  productName = p.Name,
                                  Image = p.Image,
                                  price = p.Price
                              }).ToList();
                return results;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);

            if(basket != null)
            {
                model.BasketCount = basket.basketItems.Count;

                int? basketCount = (from item in basket.basketItems
                                    select item.quantity).Sum();

                decimal? basketValue = (from item in basket.basketItems
                                        join p in productContext.Collection() on item.productID equals p.Id
                                        select item.quantity * p.Price).Sum();

                model.BasketCount = basketCount ?? 0;
                model.BasketValue = basketValue ?? decimal.Zero;

                return model;
            }
            else
            {
                return model;
            }
        }
    }
}
