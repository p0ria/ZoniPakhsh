using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Market.InfrastructureLayer.Models;
using Market.InfrastructureLayer.Repositories;
using Market.PersistenceLayer.EF.Common;

namespace Market.PersistenceLayer.EF
{
    public class OrderRepositoryEF : IOrderRepository
    {
        private zonipakhshDbContext Ctx { get { return new zonipakhshDbContext(); } }

        public InfrastructureLayer.Models.Order FindById(long id)
        {
            var _ctx = Ctx;
            return _ctx.Orders.Find(id).ToDto();
        }

        public IEnumerable<InfrastructureLayer.Models.Order> GetAll()
        {
            var _ctx = Ctx;
            return _ctx.Orders.Include(o => o.OrderItems).Include(o => o.User).ToList().Select(o => o.ToDto());
        }

        public InfrastructureLayer.Models.Order Add(InfrastructureLayer.Models.Order item)
        {
            throw new NotImplementedException();
        }

        public InfrastructureLayer.Models.Order Update(InfrastructureLayer.Models.Order item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(long id)
        {
            var _ctx = Ctx;
            try
            {
                Order tobeRemove = _ctx.Orders.Find(id);
                if (tobeRemove != null)
                {
                    _ctx.Orders.Remove(tobeRemove);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IEnumerable<InfrastructureLayer.Models.Order> GetAllOrders()
        {
            var _ctx = Ctx;
            return _ctx.Orders
                .Where(o => o.State != (short) State.Basket)
                .ToList()
                .Select(o => o.ToDto());
        }

        public InfrastructureLayer.Models.Order SubmitOrder(long userId)
        {
            var _ctx = Ctx;
            User user = _ctx.Users.Find(userId);
            if(user == null) throw new NullReferenceException("The user not found");
            Order cart = user.Orders.Single(o => o.State == (short) State.Basket);
            if(cart.OrderItems.Count == 0) throw new Exception("Cart has no item");
            bool isAvailable = true;
            foreach (OrderItem orderItem in cart.OrderItems)
            {
                if (orderItem.Product == null || orderItem.Product.Count < orderItem.Count)
                {
                    isAvailable = false;
                    break;
                }
            }
            if (isAvailable)
            {
                foreach (OrderItem orderItem in cart.OrderItems)
                {
                    orderItem.Product.Count -= orderItem.Count;
                }
                cart.State = (short) State.Submitted;
                cart.SubmitDatePersian = PersianDateTime.Now.ToString();

                user.Orders.Add(new Order
                {
                    State = (short) State.Basket
                });
                _ctx.SaveChanges();
            }
            return cart.ToDto();
        }

        public InfrastructureLayer.Models.Order GetCartForUser(long userId)
        {
            var _ctx = Ctx;
            User user = _ctx.Users.Find(userId);
            if (user == null) throw new NullReferenceException("The user not found");
            Order cart = user.Orders.SingleOrDefault(o => o.State == (short)State.Basket);
            if (cart == null)
            {
                cart = new Order
                {
                    State = (short) State.Basket
                };
                user.Orders.Add(cart);
                _ctx.SaveChanges();
            }

            return cart.ToDto();
        }

        public InfrastructureLayer.Models.Order UpdateCart(InfrastructureLayer.Models.Order order)
        {
            var _ctx = Ctx;
            User user = _ctx.Users.Find(order.User.Id);
            if (user == null) throw new NullReferenceException("The user not found");
            Order cart = user.Orders.Single(o => o.State == (short)State.Basket);

            cart.OrderItems.Clear();
            foreach (InfrastructureLayer.Models.OrderItem orderItem in order.Items)
            {
                if (orderItem.Product == null || orderItem.Product.Id == null) continue;
                Product product = _ctx.Products.Find(orderItem.Product.Id.Value);
                orderItem.Product = product.ToDto();
                if (product == null) continue;
                cart.OrderItems.Add(new OrderItem
                {
                    Product = product,
                    Count = orderItem.Count
                });
            }
            _ctx.SaveChanges();
            return order;
        }

        public InfrastructureLayer.Models.Order ChangeState(long orderId, State newState)
        {
            var _ctx = Ctx;
            Order order = _ctx.Orders.Find(orderId);
            if(order == null) throw new NullReferenceException("Order not found");
            order.State = (short)newState;
            if (newState == State.Sent)
            {
                order.SentDatePersian = PersianDateTime.Now.ToString();
            }
            if (newState == State.Delivered)
            {
                order.DeliverDatePersian = PersianDateTime.Now.ToString();
            }
            _ctx.SaveChanges();
            return order.ToDto();
        }

        public IEnumerable<InfrastructureLayer.Models.Order> GetOrdersForUser(long userId)
        {
            var _ctx = Ctx;
            return _ctx.Orders
                .Where(o => o.User != null && o.User.Id == userId && o.State != (short) State.Basket)
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .ToList()
                .Select(o => o.ToDto());
        }

        public InfrastructureLayer.Models.Order AddOrRemoveOrUpdateCartItem(long userId, InfrastructureLayer.Models.OrderItem item)
        {
            var _ctx = Ctx;
            if (item.Product == null || item.Product.Id == null) throw new NullReferenceException("Order's product cant be null");
            User user = _ctx.Users.Find(userId);
            if (user == null) throw new NullReferenceException("The user not found");
            Order cart = user.Orders.Single(o => o.State == (short)State.Basket);
            OrderItem orderItem = cart.OrderItems.FirstOrDefault(o => o.Product.Id == item.Product.Id);
            if (orderItem != null)
            {
                if (item.Count <= 0) cart.OrderItems.Remove(orderItem);
                else orderItem.Count = item.Count;
            }
            else
            {
                if (item.Count > 0)
                {
                    Product product = _ctx.Products.Find(item.Product.Id.Value);
                    if (product != null)
                    {
                        cart.OrderItems.Add(new OrderItem
                        {
                            Product = product,
                            Count = item.Count
                        });
                    }
                }
            }
            _ctx.SaveChanges();
            return cart.ToDto();
        }
    }
}