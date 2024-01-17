using ApiKeyAuth.Models;

namespace ApiKeyAuth.Services
{
    public class UserService
    {
        private readonly List<AppUser> _users = new() {
            new AppUser() { Name = "User1", ApiKey = "abc123" },
            new AppUser() { Name = "User2", ApiKey = "123abc" },
            new AppUser() { Name = "User3", ApiKey = "a1b2c3" }
        };

        public ICollection<AppUser> GetUsers()
        {
            return _users;
        }

        public AppUser? GetUserByToken(string token)
        {
            return _users.Where(x => x.ApiKey.Equals(token)).FirstOrDefault();
        }
    }
}