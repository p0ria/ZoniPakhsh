using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Market.InfrastructureLayer.Models;
using Market.InfrastructureLayer.Repositories;
using Market.PersistenceLayer.EF.Common;

namespace Market.PersistenceLayer.EF
{
    public class UserRepositoryEF : IUserRepository
    {
        private zonipakhshDbContext Ctx { get { return new zonipakhshDbContext(); } }

        public InfrastructureLayer.Models.User FindById(long id)
        {
            var _ctx = Ctx;
            return _ctx.Users.Find(id).ToDto();
        }

        public IEnumerable<InfrastructureLayer.Models.User> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public InfrastructureLayer.Models.User Add(InfrastructureLayer.Models.User item)
        {
            throw new System.NotImplementedException();
        }

        public InfrastructureLayer.Models.User Update(InfrastructureLayer.Models.User item)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(long id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<InfrastructureLayer.Models.User> GetCustomers()
        {
            var _ctx = Ctx;
            return _ctx.Users.Where(u => u.Role.Equals("customer")).ToList().Select(u => u.ToDto());
        }

        public IEnumerable<InfrastructureLayer.Models.User> GetAdmins()
        {
            var _ctx = Ctx;
            return _ctx.Users.Where(u => u.Role.Equals("administrator")).ToList().Select(u => u.ToDto());
        }

        public InfrastructureLayer.Models.UserCredential FindByUserNameAndPassword(string userName, string password)
        {
            var _ctx = Ctx;
            UserCredential userCredential = _ctx.UserCredentials
                .SingleOrDefault(uc => uc.UserName.Equals(userName) && uc.Password.Equals(password));
            return userCredential.ToDto();
        }

        public InfrastructureLayer.Models.User RegisterByPhone(string firstName, string lastName, string phone, string password, string address,
            string postalCode, string role)
        {
            var _ctx = Ctx;
            UserCredential userCredential = _ctx.UserCredentials
                .SingleOrDefault(uc => uc.UserName.Equals(phone) && uc.Password.Equals(password));
            if (userCredential != null)
            {
                throw new Exception("The phone number already exists in database.");
            }

            User newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Address = address,
                PostalCode = postalCode,
                Role = role,
            };
            newUser.UserCredential = new UserCredential
            {
                UserName = phone,
                Password = password
            };
            newUser.Orders.Add(new Order
            {
                State = (short) State.Basket
            });

            try
            {
                User addedUser = _ctx.Users.Add(newUser);
                _ctx.SaveChanges();
                return addedUser.ToDto();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}