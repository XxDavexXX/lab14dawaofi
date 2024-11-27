using Microsoft.EntityFrameworkCore;

namespace lab12.Models
{
    public class InvoiceContext :DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Grade> Grades { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAB1507-18\\SQLEXPRESS03; Database=APISem12; Integrated Security=True;Trust Server Certificate=True");
        }


    }
}
