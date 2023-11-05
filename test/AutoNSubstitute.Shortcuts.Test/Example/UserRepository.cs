using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoNSubstitute.Shortcuts.Test.Example
{
    public class UserRepository
    {
        private readonly ICollection<User> users;

        public UserRepository(ICollection<User> users) 
            => this.users = users;

        public ICollection<User> Users => users;

        public virtual User Get(int number) 
            => users.FirstOrDefault(h => h.Number == number);

        public virtual async Task<int> SaveChangesAsync() 
            => await Task.FromResult(1);
    }
}
