using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.InfrastructureLayer.Models;
using Market.InfrastructureLayer.Common;

namespace Market.InfrastructureLayer.Repositories
{
    public interface IProductRepository : IRepository<Product, long>
    {
        PageResult<Product> GetProducts(int? categoryId, int? page = 1, int? itemsPerPage = null);
    }
}
