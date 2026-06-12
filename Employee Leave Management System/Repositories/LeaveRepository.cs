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
        if (leave == null) return "Leave not found";
        leave.Status = "Approved";
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
        return "Leave approved successfully";
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
    public async Task<IEnumerable<EmployeeResponseDto>> GetEmployeesCurrentlyOnLeave()
    {
        return await _dbContext.LeaveRequests
            .Where(l => l.Status == "Approved")
            .Select(l => new EmployeeResponseDto
            {
                Id = l.Employee.Id,
                FullName = l.Employee.FullName,
                Email = l.Employee.Email,
                Department = l.Employee.Department
            })
            .ToListAsync();
    }
    
    // Get Leave Statistics
    public async Task<object> GetLeaveStatistics()
    {
        var stats = await _dbContext.LeaveRequests
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

        return stats;
    }
}