using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.InfrastructureLayer.Models;

namespace Market.PersistenceLayer.EF.Common
{
    public static class ConvertExtensionMethods
    {
        public static InfrastructureLayer.Models.User ToDto(this User @this)
        {
            if (@this == null) return null;
            return new InfrastructureLayer.Models.User
            {
                Id = @this.Id,
                FirstName = @this.FirstName,
                LastName = @this.LastName,
                Address = @this.Address,
                Phone = @this.Phone,
                PostalCode = @this.PostalCode,
                Role = @this.Role
            };
        }

        public static InfrastructureLayer.Models.UserCredential ToDto(this UserCredential @this)
        {
            if (@this == null) return null;
            return new InfrastructureLayer.Models.UserCredential
            {
                User = @this.User.ToDto(),
                UserName = @this.UserName,
                Password = @this.Password
            };
        }

        public static InfrastructureLayer.Models.Category ToDto(this Category @this)
        {
            if (@this == null) return null;
            return new InfrastructureLayer.Models.Category
            {
                Id = @this.Id,
                ImageUrlRelative = @this.ImageUrlRelative,
                ImageUrl = Constants.BaseAddress + @this.ImageUrlRelative,
                Name = @this.Name
            };
        }

        public static Category ToEF(this InfrastructureLayer.Models.Category @this)
        {
            return new Category
            {
                Id = @this.Id ?? default(int),
                Name = @this.Name,
                ImageUrlRelative = @this.ImageUrlRelative
            };
        }

        public static InfrastructureLayer.Models.Product ToDto(this Product @this)
        {
            if (@this == null) return null;
            return new InfrastructureLayer.Models.Product
            {
                Id = @this.Id,
                Name = @this.Name,
                Count = @this.Count,
                Price = @this.Price,
                ImageUrlRelative = @this.ImageUrlRelative,
                ImageUrl = Constants.BaseAddress + @this.ImageUrlRelative,
                Category = @this.Category.ToDto()
            };
        }

        public static Product ToEF(this InfrastructureLayer.Models.Product @this)
        {
            return new Product
            {
                Id = @this.Id ?? default(long),
                Name = @this.Name,
                Price = @this.Price,
                Count = @this.Count,
                ImageUrlRelative = @this.ImageUrlRelative
            };
        }

        public static InfrastructureLayer.Models.OrderItem ToDto(this OrderItem @this)
        {
            if (@this == null) return null;
            return new InfrastructureLayer.Models.OrderItem
            {
                Product = @this.Product.ToDto(),
                Count = @this.Count
            };
        }

        public static InfrastructureLayer.Models.Order ToDto(this Order @this)
        {
            if (@this == null) return null;
            InfrastructureLayer.Models.Order order = new InfrastructureLayer.Models.Order
            {
                Id = @this.Id,
                User = @this.User.ToDto(),
                State = (State) @this.State,
                SubmitDatePersian = @this.SubmitDatePersian,
                SentDatePersian = @this.SentDatePersian,
                DeliverDatePersian = @this.DeliverDatePersian
            };
            order.Items.AddRange(@this.OrderItems.Select(oi => oi.ToDto()));
            return order;
        }

    }
}
