using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Market.InfrastructureLayer.Models;

namespace Market.GateWayLayer.Rest.Common
{
    public static class AuthUtils
    {
        public static User ToUser(IPrincipal principal)
        {
            ClaimsIdentity identityClaims = (ClaimsIdentity)principal.Identity;
            long id = long.Parse(identityClaims.FindFirst("Id").Value);
            User user = Repository.UserRepository.FindById(id);
            return user;
        }
    }
}