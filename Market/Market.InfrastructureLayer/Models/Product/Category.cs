using System.Collections.Generic;

namespace Market.InfrastructureLayer.Models
{
    public class Category
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlRelative { get; set; }
    }
}