using System.Threading.Tasks;
using System.Collections.Generic;
using TaskTrackerData.Model;

namespace TaskTrackerData.Repository
{
    public class TaskRepository : BaseRepository
    {
        public TaskRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public TaskObject New()
        {
            return new TaskObject(ApplicationContext);
        }

        public TaskObject Get(int tskId)
        {
            return new TaskObject(ApplicationContext, tskId);
        }

        public List<TaskObject> GetList()
        {
            return new TaskObject(ApplicationContext).GetList();
        }

        public Task<List<TaskObject>> GetListAsync()
        {
            return new TaskObject(ApplicationContext).GetListAsync();
        }
    }
}
