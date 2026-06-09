using Employee_Leave_Management_System.Model;
using Microsoft.EntityFrameworkCore;

namespace Employee_Leave_Management_System.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    public DbSet<LeaveApproval> LeaveApprovals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LeaveRequest>()
            .HasOne(l => l.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(l => l.EmployeeId);

        modelBuilder.Entity<LeaveApproval>()
            .HasOne(a => a.LeaveRequest)
            .WithMany(l => l.Approvals)
            .HasForeignKey(a => a.LeaveRequestId);

        base.OnModelCreating(modelBuilder);
    }
}