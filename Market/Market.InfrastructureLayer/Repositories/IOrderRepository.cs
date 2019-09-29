using System.Collections.Generic;
using Market.InfrastructureLayer.Models;

namespace Market.InfrastructureLayer.Repositories
{
    public interface IOrderRepository : IRepository<Order, long>
    {
        IEnumerable<Order> GetAllOrders();
        Order SubmitOrder(long userId);

        Order GetCartForUser(long userId);

        Order UpdateCart(Order order);

        Order ChangeState(long orderId, State state);

        IEnumerable<Order> GetOrdersForUser(long userId);
        Order AddOrRemoveOrUpdateCartItem(long userId, OrderItem item);
    }
}