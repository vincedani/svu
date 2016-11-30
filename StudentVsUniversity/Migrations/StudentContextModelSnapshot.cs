using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using StudentVsUniversity.Models.DataContext;

namespace StudentVsUniversity.Migrations
{
    [DbContext(typeof(StudentContext))]
    partial class StudentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("StudentVsUniversity.Models.Activity", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int>("EllapsedMinutes");

                    b.Property<int>("EllapsedRestTime");

                    b.Property<int>("EllapsedWorkTime");

                    b.Property<string>("Name");

                    b.Property<int>("RestTime");

                    b.Property<int>("WorkTime");

                    b.HasKey("ID");

                    b.ToTable("Acivities");
                });
        }
    }
}
