

namespace TaskTrackerData.Repository
{
    public abstract class BaseRepository
    {
        protected ApplicationContext ApplicationContext { get; set; }

        public BaseRepository(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
        }


    }

}
