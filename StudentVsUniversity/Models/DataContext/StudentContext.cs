using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentVsUniversity.Models.DataContext
{
    public class StudentContext : DbContext
    {
        public DbSet<Activity> Acivities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=StudentDbVince.db");
        }
    }
}
