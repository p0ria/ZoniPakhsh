using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Market.InfrastructureLayer.Common;
using Market.InfrastructureLayer.Models;
using Market.InfrastructureLayer.Repositories;
using Market.PersistenceLayer.EF.Common;

namespace Market.PersistenceLayer.EF
{
    public class ProductRepositoryEF : IProductRepository
    {
        private zonipakhshDbContext Ctx { get { return new zonipakhshDbContext(); } }

        public InfrastructureLayer.Models.Product FindById(long id)
        {
            var _ctx = Ctx;
            return _ctx.Products.Find(id).ToDto();
        }

        public IEnumerable<InfrastructureLayer.Models.Product> GetAll()
        {
            var _ctx = Ctx;
            return _ctx.Products.Include(p => p.Category).ToList().Select(p => p.ToDto());
        }

        public InfrastructureLayer.Models.Product Add(InfrastructureLayer.Models.Product item)
        {
            var _ctx = Ctx;
            if (item.Category?.Id == null) throw new NullReferenceException("Product Category can not be null");
            Category category = _ctx.Categories.Find(item.Category.Id.Value);
            if(category == null) throw new NullReferenceException("Product Category Not Found");
            Product newProduct = item.ToEF();
            category.Products.Add(newProduct);
            _ctx.SaveChanges();
            return newProduct.ToDto();
        }

        public InfrastructureLayer.Models.Product Update(InfrastructureLayer.Models.Product item)
        {
            var _ctx = Ctx;
            if (item.Id == null) throw new NullReferenceException("Product Id can not be null");
            if (item.Category?.Id == null) throw new NullReferenceException("Product Category can not be null");
            Product tobeUpdate = _ctx.Products.Find(item.Id);
            if(tobeUpdate == null) throw new NullReferenceException("Product not found");
            tobeUpdate.Name = item.Name;
            tobeUpdate.Price = item.Price;
            tobeUpdate.Count = item.Count;
            tobeUpdate.ImageUrlRelative = item.ImageUrlRelative;
            Category category = _ctx.Categories.Find(item.Category.Id);
            if (category != null)
            {
                tobeUpdate.Category = category;
            }
            _ctx.SaveChanges();
            return tobeUpdate.ToDto();
        }

        public bool Remove(long id)
        {
            var _ctx = Ctx;
            Product tobeRemove = _ctx.Products.Find(id);
            if (tobeRemove == null) return true;
            try
            {
                _ctx.Products.Remove(tobeRemove);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                FileLogger.Log(e.ToString());
                return false;
            }
        }

        public PageResult<InfrastructureLayer.Models.Product> GetProducts(int? categoryId, int? page, int? itemsPerPage = null)
        {
            var _ctx = Ctx;
            page = page ?? 1;
            IEnumerable<InfrastructureLayer.Models.Product> productsInPage;
            try
            {
                IEnumerable<InfrastructureLayer.Models.Product> allProductsInCategory =
                    _ctx.Products
                        .Where(p => categoryId == null || (p.Category != null && p.Category.Id == categoryId))
                        .Include(p => p.Category)
                        .ToList()
                        .Select(p => p.ToDto());

                if (itemsPerPage == null)
                {
                    productsInPage = allProductsInCategory;
                    itemsPerPage = allProductsInCategory.Count();
                }
                else
                {
                    productsInPage =
                        allProductsInCategory.Skip((page.Value - 1) * itemsPerPage.Value)
                            .Take(itemsPerPage.Value);
                }

                var result = new PageResult<InfrastructureLayer.Models.Product>
                {
                    Items = productsInPage,
                    Page = page.Value,
                };
                if (allProductsInCategory != null && allProductsInCategory.Count() != 0)
                {
                    result.TotalPages =
                        (int)Math.Ceiling(((decimal)allProductsInCategory.Count()) /
                                          ((decimal)itemsPerPage));
                }
                else
                {
                    result.Page = 0;
                    result.TotalPages = 0;
                }

                return result;
            }
            catch (Exception e)
            {
                return new PageResult<InfrastructureLayer.Models.Product>();
            }
        }
    }
}