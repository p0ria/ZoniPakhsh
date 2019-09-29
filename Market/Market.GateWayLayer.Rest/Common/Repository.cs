using System;
using Market.InfrastructureLayer.Repositories;
using Market.PersistenceLayer.EF;

namespace Market.GateWayLayer.Rest.Common
{
    public static class Repository
    {
        static Repository()
        {
            try
            {
                UserRepository = new UserRepositoryEF();
                CategoryRepository = new CategoryRepositoryEF();
                ProductRepository = new ProductRepositoryEF();
                OrderRepository = new OrderRepositoryEF();
            }
            catch (Exception e)
            {
                FileLogger.Log(e.ToString());
                throw;
            }
            
        }

        public static ICategoryRepository CategoryRepository { get; private set; }
        public static IOrderRepository OrderRepository { get; private set; }
        public static IProductRepository ProductRepository { get; private set; }
        public static IUserRepository UserRepository { get; private set; }
    }
}