
using System;
using System.Collections.Generic;
using System.Text;
using TaskTrackerData;
using TaskTrackerData.Repository;

namespace TaskTrackerData.Repository
{
    public class CommonRepository : BaseRepository
    {
        public CommonRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public UserRepository User { get { return new UserRepository(ApplicationContext); } }
    }
}
