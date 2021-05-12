using System.Threading.Tasks;
using System.Collections.Generic;
using TaskTrackerData.Model;

namespace TaskTrackerData.Repository
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public User New()
        {
            return new User(ApplicationContext);
        }

        public User Get(int bsuId)
        {
            return new User(ApplicationContext, bsuId);
        }

        public List<User> GetList()
        {
            return new User(ApplicationContext).GetList();
        }

        public Task<List<User>> GetListAsync()
        {
            return new User(ApplicationContext).GetListAsync();
        }
    }
}
