using System;

namespace Market.InfrastructureLayer.Models
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Count { get; set; }

        public decimal TotalPrice
        {
            get
            {
                if (Product == null) return 0;
                return Product.Price * Count;
            }
        }

        public bool Available
        {
            get { return Product != null && Product.Id != null && Count <= Product.Count; }
        }
    }
}