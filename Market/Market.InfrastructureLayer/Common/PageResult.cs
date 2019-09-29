using System.Collections.Generic;

namespace Market.InfrastructureLayer.Common
{
    public class PageResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}