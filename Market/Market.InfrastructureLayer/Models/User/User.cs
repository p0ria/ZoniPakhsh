using System.Collections.Generic;
using Market.InfrastructureLayer.Common;

namespace Market.InfrastructureLayer.Models
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " +  LastName; } }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Role { get; set; }
    }
}
