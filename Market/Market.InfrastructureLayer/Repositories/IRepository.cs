using System.Collections.Generic;

namespace Market.InfrastructureLayer.Repositories
{
    public interface IRepository<TItem, TId>
    {
        TItem FindById(TId id);
        IEnumerable<TItem> GetAll();
        TItem Add(TItem item);
        TItem Update(TItem item);
        bool Remove(TId id);
    }
}