namespace Market.InfrastructureLayer.Models
{
    public class UserCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public User User { get; set; }
    }
}