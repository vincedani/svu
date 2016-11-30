using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentVsUniversity.Models;
using StudentVsUniversity.Settings;

namespace StudentVsUniversity.ViewModels
{
    class ActivityViewModel
    {
        public List<MappedActivity> Activities { get; set; }
        public MappedActivity SelectedActivity { get; set; }
        public StudentSettings StudentSettings { get; set; }

    }
}
