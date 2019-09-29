using System.Collections.Generic;
using Market.InfrastructureLayer.Models;

namespace Market.InfrastructureLayer.Repositories
{
    public interface IUserRepository : IRepository<User, long>
    {
        IEnumerable<User> GetCustomers();

        IEnumerable<User> GetAdmins();

        UserCredential FindByUserNameAndPassword(string userName, string password);

        User RegisterByPhone(string firstName, string lastName, string phone, string password, string address, string postalCode, string role);
    }
}