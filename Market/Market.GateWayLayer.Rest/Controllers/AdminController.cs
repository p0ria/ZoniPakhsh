using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Market.GateWayLayer.Rest.Common;
using Market.GateWayLayer.Rest.Models;
using Market.InfrastructureLayer.Models;

namespace Market.GateWayLayer.Rest.Controllers
{
    [RoutePrefix("api/admins")]
    [Authorize(Roles = "administrator")]
    public class AdminController : ApiController
    {
        [HttpGet]
        [Route]
        public HttpResponseMessage GetAdmins()
        {
            IEnumerable<User> users = Repository.UserRepository.GetAdmins();
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        [HttpPost]
        [Route]
        public HttpResponseMessage RegisterAdmin(UserJson info)
        {
            if (string.IsNullOrEmpty(info.Phone))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter 'Phone' can not be empty");
            }

            User newUser = Repository.UserRepository.RegisterByPhone(
                info.FirstName, info.LastName, info.Phone, info.Password, info.Address, info.PostalCode, "administrator");
            return Request.CreateResponse(HttpStatusCode.OK, newUser);
        }
    }
}