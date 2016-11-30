using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace StudentVsUniversity.Models
{
    public class MappedActivity
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public int WorkTime { get; set; } = 25;
        public int RestTime { get; set; } = 5;
        public DateTime CreatedTime { get; set; }
        public int EllapsedMinutes { get; set; }
        public int EllapsedWorkTime { get; set; }
        public int EllapsedRestTime { get; set; }
    }
}
