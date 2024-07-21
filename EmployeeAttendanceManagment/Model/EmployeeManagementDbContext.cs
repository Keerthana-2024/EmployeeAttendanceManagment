using EmployeeAttendanceManagment.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendanceManagement.Model
{
    public class EmployeeManagementDbContext : DbContext
    {
        public EmployeeManagementDbContext(DbContextOptions<EmployeeManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<OrganizationChart> OrganizationCharts { get; set; }
        public DbSet<HolidayCalendar> HolidayCalendars { get; set; }
        public DbSet<AttendancePolicy> AttendancePolicies { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships, constraints, etc.
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict); // or another delete behavior as needed

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.AttendancePolicy)
                .WithMany()
                .HasForeignKey(e => e.AttendancePolicyID)
                .OnDelete(DeleteBehavior.Restrict); // or another delete behavior as needed

            // Relationship for Manager (self-referencing)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey(e => e.ManagerID)
                .OnDelete(DeleteBehavior.Restrict); // or another delete behavior as needed

            // Configure Leave relationships
            modelBuilder.Entity<Leave>()
                .HasOne(l => l.Employee)
                .WithMany(e => e.Leaves)
                .HasForeignKey(l => l.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade); // Ensure that when an employee is deleted, the associated leaves are also deleted

            base.OnModelCreating(modelBuilder);
        }
    }
}
