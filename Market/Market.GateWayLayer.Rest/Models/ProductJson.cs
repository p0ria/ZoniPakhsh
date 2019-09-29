namespace Market.GateWayLayer.Rest.Models
{
    public class ProductJson
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public long Price { get; set; }
        public int Count { get; set; }
        public string ImageData { get; set; }
        public string ImageType { get; set; }
    }
}