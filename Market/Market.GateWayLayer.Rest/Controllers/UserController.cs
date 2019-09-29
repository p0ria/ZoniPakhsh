using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Market.GateWayLayer.Rest.Common;
using Market.GateWayLayer.Rest.Models;
using Market.InfrastructureLayer.Models;

namespace Market.GateWayLayer.Rest.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "administrator")]
        [Route]
        public HttpResponseMessage GetUsers()
        {
            IEnumerable<User> users = Repository.UserRepository.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route]
        public HttpResponseMessage RegisterUser(UserJson info)
        {
            try
            {
                if (string.IsNullOrEmpty(info.Phone))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter 'Phone' can not be empty");
                }

                User newUser = Repository.UserRepository.RegisterByPhone(
                    info.FirstName, info.LastName, info.Phone, info.Password, info.Address, info.PostalCode, "customer");
                return Request.CreateResponse(HttpStatusCode.OK, newUser);
            }
            catch (Exception e)
            {
                FileLogger.Log(e.ToString());
                throw;
            }
        }
    }
}