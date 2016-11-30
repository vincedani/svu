using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace StudentVsUniversity.Models
{
    /// <summary>
    /// Represents the activity (study / work).
    /// </summary>
    public class Activity
    {
        [Required]
        [Key]
        public long ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public int WorkTime { get; set; } = 25;

        [Required]
        public int RestTime { get; set; } = 5;

        [Required]
        public DateTime CreatedTime { get; set; }

        public int EllapsedMinutes { get; set; }
        public int EllapsedWorkTime { get; set; }
        public int EllapsedRestTime { get; set; }
    }
}
