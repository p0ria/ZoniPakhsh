using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Market.GateWayLayer.Rest.Common;
using Market.InfrastructureLayer.Models;

namespace Market.GateWayLayer.Rest.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrderController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "administrator")]
        [Route]
        public HttpResponseMessage GetAll()
        {
            IEnumerable<Order> orders = Repository.OrderRepository.GetAllOrders();
            return Request.CreateResponse(HttpStatusCode.OK, orders);
        }

        [HttpPut]
        [Authorize(Roles = "administrator")]
        [Route("{id}")]
        public HttpResponseMessage ChangeState(int id, [FromUri]int state)
        {
            Order order = Repository.OrderRepository.FindById(id);
            if (order == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found");
            if (order.State == State.Basket)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cant change the state of Basket");
            if (state != 2 && state != 3) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Illegal state");
            order = Repository.OrderRepository.ChangeState(id, (State)state);

            return Request.CreateResponse(HttpStatusCode.OK, order);
        }

        [HttpDelete]
        [Authorize(Roles = "administrator")]
        [Route("{id}")]
        public HttpResponseMessage DeleteOrder(int id)
        {
            bool succeed = Repository.OrderRepository.Remove(id);
            return Request.CreateResponse(HttpStatusCode.OK, succeed);
        }
    }
}