using System.Threading.Tasks;

namespace AutoNSubstitute.Shortcuts.Test.Example
{
    public class UserService
    {
        private readonly UserRepository repository;

        public UserService(UserRepository repository) 
            => this.repository = repository;
    
        public virtual User Get(int number) 
            => repository.Get(number);

        public virtual async Task<User> GetAsync(int number) 
            => await Task.FromResult(repository.Get(number));

        public UserRepository Repository => repository;
    }
}
