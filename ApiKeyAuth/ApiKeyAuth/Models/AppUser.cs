namespace ApiKeyAuth.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ApiKey { get; set; }

        public AppUser()
        {
            Name = string.Empty;
            ApiKey = string.Empty;
        }
    }
}