using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentVsUniversity.Helpers;
using StudentVsUniversity.Models;
using StudentVsUniversity.Models.DataContext;

namespace StudentVsUniversity.Services
{
    public class LocalStorageService : Interfaces.IStorageService
    {
        #region Fields
        private StudentContext context = new StudentContext();
        #endregion

        #region Interface implementation

        public MappedActivity GetActivityById(long id)
        {
            Activity activity = context.Acivities.SingleOrDefault(x => x.ID == id);
            return ActivityMapper.MapActivityFromDB(activity);
        }

        public MappedActivity AddActivity(MappedActivity activity)
        {
            Activity act = ActivityMapper.MapActivityToDB(activity);

            act.CreatedTime = DateTime.Now;
            act = context.Acivities.Add(act).Entity;
            context.SaveChanges();

            return ActivityMapper.MapActivityFromDB(act);
        }

        public void DeleteActivity(MappedActivity activity)
        {
            Activity act = ActivityMapper.MapActivityToDB(activity);

            context.Acivities.Attach(act);
            context.Acivities.Remove(act);
            context.SaveChanges();
        }

        public IEnumerable<MappedActivity> GetActivities()
        {
            var activities = context.Acivities;
            List<MappedActivity> ret = new List<MappedActivity>();

            foreach (Activity a in activities)
            {
                ret.Add(ActivityMapper.MapActivityFromDB(a));
            }
            return ret;
        }


        public MappedActivity ModifActivity(MappedActivity activity)
        {
            Activity act = ActivityMapper.MapActivityToDB(activity);

            context.Entry(act).State = EntityState.Modified;
            context.SaveChanges();

            act = context.Acivities.SingleOrDefault(a => a.ID == activity.ID);
            return ActivityMapper.MapActivityFromDB(act);
        }

        public void MigrateDatabase()
        {
            context.Database.Migrate();
        }

        public void SwapDatabase()
        {
            context.Database.ExecuteSqlCommand("delete from Acivities");
        }
        #endregion
    }
}
