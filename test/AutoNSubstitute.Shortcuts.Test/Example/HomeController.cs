using System.Threading.Tasks;

namespace AutoNSubstitute.Shortcuts.Test.Example
{
    public class HomeController
    {
        private readonly UserService service;

        public HomeController(UserService service) 
            => this.service = service;

        public User GetUser(int number) 
            => service.Get(number);

        public async Task<User> GetUserAsync(int number)
            => await service.GetAsync(number);
    
        public UserService Service => service;
    }
}
