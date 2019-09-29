using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Market.InfrastructureLayer.Models
{
    public class Product
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public long Price { get; set; }
        public string ImageUrlRelative { get; set; }
        public string ImageUrl { get; set; }
        public int Count { get; set; }
    }
}