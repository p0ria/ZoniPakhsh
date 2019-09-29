using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Market.GateWayLayer.Rest.Common;
using Market.InfrastructureLayer.Models;

namespace Market.GateWayLayer.Rest.Controllers
{
    [RoutePrefix("api/currentUser")]
    public class CurrentUserController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route]
        public HttpResponseMessage GetUser()
        {
            User user = AuthUtils.ToUser(User);
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [HttpGet]
        [Authorize]
        [Route("cart")]
        public HttpResponseMessage GetUserCart()
        {
            User user = AuthUtils.ToUser(User);
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found");
            }
            Order order = Repository.OrderRepository.GetCartForUser(user.Id);
            if (order == null) Request.CreateErrorResponse(HttpStatusCode.NotFound, "The user's cart not found!");
            return Request.CreateResponse(HttpStatusCode.OK, order);
        }

        [HttpPut]
        [Authorize]
        [Route("cart")]
        public HttpResponseMessage UpdateUserCart(Order order)
        {
            if (order.Id == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Order 'Id' can not be null");
            User user = AuthUtils.ToUser(User);
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found");
            }
            if (!VerifyCart(order))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cart is not valid");
            }
            Order cart = Repository.OrderRepository.GetCartForUser(user.Id);
            if (cart.Id.Value != order.Id.Value)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "This cart is not for current user");
            }
            cart = Repository.OrderRepository.UpdateCart(order);
            return Request.CreateResponse(HttpStatusCode.OK, cart);
        }

        [HttpPost]
        [Authorize]
        [Route("cart/items")]
        public HttpResponseMessage AddItemToCart(OrderItem item)
        {
            User user = AuthUtils.ToUser(User);
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found");
            }
            Order cart = Repository.OrderRepository.AddOrRemoveOrUpdateCartItem(user.Id, item);
            return Request.CreateResponse(HttpStatusCode.OK, cart);
        }

        [HttpGet]
        [Authorize]
        [Route("orders")]
        public HttpResponseMessage GetUserOrders()
        {
            User user = AuthUtils.ToUser(User);
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found");
            }
            IEnumerable<Order> orders = Repository.OrderRepository.GetOrdersForUser(user.Id);
            return Request.CreateResponse(HttpStatusCode.OK, orders);
        }

        [HttpPost]
        [Authorize]
        [Route("orders")]
        public HttpResponseMessage SubmitOrder()
        {
            User user = AuthUtils.ToUser(User);
            Order order = Repository.OrderRepository.SubmitOrder(user.Id);
            return Request.CreateResponse(HttpStatusCode.OK, order);
        }

        private bool VerifyCart(Order order)
        {
            if (order.State != State.Basket) return false;
            if (order.Id == null) return false;
            return true;
        }
    }
}