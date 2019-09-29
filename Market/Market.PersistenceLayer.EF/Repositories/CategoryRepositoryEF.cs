using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.InfrastructureLayer.Repositories;
using Market.PersistenceLayer.EF.Common;

namespace Market.PersistenceLayer.EF
{
    public class CategoryRepositoryEF : ICategoryRepository
    {
        private zonipakhshDbContext Ctx{get{return new zonipakhshDbContext();}}

        public InfrastructureLayer.Models.Category FindById(int id)
        {
            var _ctx = Ctx;
            return _ctx.Categories.Find(id).ToDto();
        }

        public IEnumerable<InfrastructureLayer.Models.Category> GetAll()
        {
            var _ctx = Ctx;
            return _ctx.Categories.ToList().Select(c => c.ToDto());
        }

        public InfrastructureLayer.Models.Category Add(InfrastructureLayer.Models.Category item)
        {
            var _ctx = Ctx;
            Category addedItem = _ctx.Categories.Add(item.ToEF());
            _ctx.SaveChanges();
            return addedItem.ToDto();
        }

        public InfrastructureLayer.Models.Category Update(InfrastructureLayer.Models.Category item)
        {
            var _ctx = Ctx;
            if (item.Id == null) throw new NullReferenceException("Category Id can not be null");
            Category tobeUpdate = _ctx.Categories.Find(item.Id.Value);
            if (tobeUpdate == null) return null;
            tobeUpdate.Name = item.Name;
            tobeUpdate.ImageUrlRelative = item.ImageUrlRelative;
            _ctx.SaveChanges();
            return tobeUpdate.ToDto();
        }

        public bool Remove(int id)
        {
            var _ctx = Ctx;
            Category tobeRemove = _ctx.Categories.Find(id);
            if (tobeRemove == null) return true;
            if (tobeRemove.Products.Count > 0) return false;
            try
            {
                _ctx.Categories.Remove(tobeRemove);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
