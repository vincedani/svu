using System.Collections;
using System.Collections.Generic;
using StudentVsUniversity.Models;

namespace StudentVsUniversity.Services.Interfaces
{
    interface IStorageService
    {
        MappedActivity GetActivityById(long id);
        MappedActivity AddActivity(MappedActivity activity);
        MappedActivity ModifActivity(MappedActivity activity);
        void DeleteActivity(MappedActivity activity);
        IEnumerable<MappedActivity> GetActivities();
        void MigrateDatabase();
    }
}
