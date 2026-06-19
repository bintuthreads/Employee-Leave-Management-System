using Employee_Leave_Management_System.Data;
using Employee_Leave_Management_System.Model;
using Employee_Leave_Management_System.Model.DTOs;
using Employee_Leave_Management_System.Model.DTOs.Requests;
using Employee_Leave_Management_System.Model.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace Employee_Leave_Management_System.Repositories;

public class LeaveRepository : ILeaveRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LeaveRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //GET ALL LEAVES
    public async Task<IEnumerable<LeaveRequestResponseDto>> GetAllLeaves()
    {
        return await _dbContext.LeaveRequests
            .Select(l => new LeaveRequestResponseDto
            {
                Id = l.Id,
                EmployeeId = l.EmployeeId,
                LeaveType = l.LeaveType,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Reason = l.Reason,
                Status = l.Status,
                DateCreated = l.DateCreated
            })
            .ToListAsync();
    }

    //GET LEAVE BY ID
    public async Task<LeaveRequestResponseDto?> GetLeaveById(int id)
    {
        var leave = await _dbContext.LeaveRequests
            .Where(l => l.Id == id)
            .Select(l => new LeaveRequestResponseDto
            {
                Id = l.Id,
                EmployeeId = l.EmployeeId,
                LeaveType = l.LeaveType,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Reason = l.Reason,
                Status = l.Status,
                DateCreated = l.DateCreated
            })
            .FirstOrDefaultAsync();
        if (leave == null)
        {
            return null;
        }
        return leave;
    }

    //SUBMIT LEAVE
    public async Task<string> SubmitLeave(SubmitLeaveRequestDto dto)
    {
        var employeeExists = await _dbContext.Employees.AnyAsync(e => e.Id == dto.EmployeeId);

        if (!employeeExists)
            return "Employee does not exist";

        int days = (dto.EndDate - dto.StartDate).Days + 1;

        switch (dto.LeaveType)
        {
            case LeaveType.Annual:
                if (days > 30)
                    return "Annual leave cannot exceed 30 days";
                break;

            case LeaveType.Sick:
                if (days > 14)
                    return "Sick leave cannot exceed 14 days";
                break;

            case LeaveType.Maternity:
                if (days > 90)
                    return "Maternity leave cannot exceed 90 days";
                break;

            case LeaveType.Paternity:
                if (days > 14)
                    return "Paternity leave cannot exceed 14 days";
                break;

            case LeaveType.Unpaid:
                if (days > 60)
                    return "Unpaid leave cannot exceed 60 days";
                break;

            case LeaveType.Emergency:
                if (days > 7)
                    return "Emergency leave cannot exceed 7 days";
                break;
            
            case LeaveType.Study:
                if (days > 120)
                    return "Study leave cannot exceed 120 days";
                break;
        }

        var overlap = await _dbContext.LeaveRequests.AnyAsync(l =>
            l.EmployeeId == dto.EmployeeId &&
            l.StartDate <= dto.EndDate &&
            l.EndDate >= dto.StartDate);
        if (overlap)
            return "Overlapping leave request exists";

        var leave = new LeaveRequest
        {
            EmployeeId = dto.EmployeeId,
            LeaveType = dto.LeaveType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Reason = dto.Reason,
            Status = "Pending",
            DateCreated = DateTime.UtcNow
        };

        await _dbContext.LeaveRequests.AddAsync(leave);
        await _dbContext.SaveChangesAsync();
        return "Leave submitted successfully";
    }
    
    // UPDATE LEAVE
    public async Task<string> UpdateLeave(int id, SubmitLeaveRequestDto dto)
    {
        var leave = await _dbContext.LeaveRequests.FindAsync(id);
        if (leave == null) return "Leave not found";

        leave.LeaveType = dto.LeaveType;
        leave.StartDate = dto.StartDate;
        leave.EndDate = dto.EndDate;
        leave.Reason = dto.Reason;
        await _dbContext.SaveChangesAsync();
        return "Leave updated successfully";
    }

    //DELETE LEAVE
    public async Task<string> DeleteLeave(int id)
    {
        var leave = await _dbContext.LeaveRequests.FindAsync(id);
        if (leave == null) return "Leave not found";

        _dbContext.LeaveRequests.Remove(leave);
        await _dbContext.SaveChangesAsync();
        return "Leave deleted successfully";
    }

    
    //APPROVE LEAVE
    public async Task<string> ApproveLeave(int id, LeaveActionRequestDto dto)
    {
        var leave = await _dbContext.LeaveRequests.FindAsync(id);
        if (leave == null)
            return "Leave not found";
        
        // Check if this approver has already approved
        var alreadyApproved = await _dbContext.LeaveApprovals
            .AnyAsync(a => a.LeaveRequestId == id && a.ApproverId == dto.ApproverId);
        if (alreadyApproved)
            return "You have already approved this leave request";

        // Add approval
        var approval = new LeaveApproval
        {
            LeaveRequestId = id,
            ApproverId = dto.ApproverId,
            Action = "Approved",
            Reason = dto.Reason,
            DateActed = DateTime.UtcNow
        };
        await _dbContext.LeaveApprovals.AddAsync(approval);
        await _dbContext.SaveChangesAsync();

        // Count approvals for this leave
        var approvalCount = await _dbContext.LeaveApprovals
            .CountAsync(a => a.LeaveRequestId == id && a.Action == "Approved");

        if (approvalCount == 1)
        {
            leave.Status = "Processing";
        }
        else if (approvalCount >= 2)
        {
            leave.Status = "Approved";
        }
        await _dbContext.SaveChangesAsync();
        return $"Leave has {approvalCount} approval(s)";
    }

    //REJECT LEAVE
    public async Task<string> RejectLeave(int id, LeaveActionRequestDto dto)
    {
        var leave = await _dbContext.LeaveRequests.FindAsync(id);
        if (leave == null) return "Leave not found";

        leave.Status = "Rejected";
        var approval = new LeaveApproval
        {
            LeaveRequestId = id,
            ApproverId = dto.ApproverId,
            Action = "Rejected",
            Reason = dto.Reason,
            DateActed = DateTime.UtcNow
        };
        await _dbContext.LeaveApprovals.AddAsync(approval);
        await _dbContext.SaveChangesAsync();
        return "Leave rejected successfully";
    }
    
    //GET LEAVE BY STATUS
    public async Task<IEnumerable<LeaveRequestResponseDto>> GetLeavesByStatus(string status)
    {
        return await _dbContext.LeaveRequests
            .Where(l => l.Status == status)
            .Select(l => new LeaveRequestResponseDto
            {
                Id = l.Id,
                EmployeeId = l.EmployeeId,
                LeaveType = l.LeaveType,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Reason = l.Reason,
                Status = l.Status,
                DateCreated = l.DateCreated
            })
            .ToListAsync();
    }

    //GET EMPLOYEES CURRENTLY ON LEAVE
    public async Task<List<Employee>> GetEmployeesCurrentlyOnLeave(string department)
    {
        var today = DateTime.UtcNow.Date;

        return await _dbContext.LeaveRequests
            .Where(l =>
                l.Status == "Approved" &&
                l.StartDate <= today &&
                l.EndDate >= today &&
                l.Employee.Department == department)
            .Select(l => l.Employee)
            .ToListAsync();
    }
    
    // Get Leave Statistics
    public async Task<object> GetLeaveStatistics()
    {
        var today = DateTime.UtcNow.Date;

        // 1. OVERALL STATS
        var totalEmployees = await _dbContext.Employees.CountAsync();
        var totalLeaves = await _dbContext.LeaveRequests.CountAsync();
        var currentlyOnLeave = await _dbContext.LeaveRequests
            .CountAsync(l =>
                l.Status == "Approved" &&
                l.StartDate <= today &&
                l.EndDate >= today);

        // 2. DEPARTMENT BREAKDOWN
        var byDepartment = await _dbContext.LeaveRequests
            .Include(l => l.Employee)
            .GroupBy(l => l.Employee.Department)
            .Select(g => new
            {
                Department = g.Key,
                TotalLeaves = g.Count(),
                Approved = g.Count(x => x.Status == "Approved"),
                Pending = g.Count(x => x.Status == "Pending"),
                Rejected = g.Count(x => x.Status == "Rejected")
            })
            .ToListAsync();

        // 3. RETURN EVERYTHING
        return new
        {
            totalEmployees,
            totalLeaves,
            currentlyOnLeave,
            byDepartment
        };
    }
}