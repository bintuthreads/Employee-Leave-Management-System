using Employee_Leave_Management_System.Data;
using Employee_Leave_Management_System.Model;
using Microsoft.EntityFrameworkCore;

namespace Employee_Leave_Management_System.Repositories;

public class LeaveRepository : ILeaveRepository
{
     private readonly ApplicationDbContext _context;

    public LeaveRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LeaveRequest> SubmitLeaveRequest(LeaveRequest leaveRequest)
    {
        await _context.LeaveRequests.AddAsync(leaveRequest);

        await _context.SaveChangesAsync();

        return leaveRequest;
    }

    public async Task<List<LeaveRequest>> GetAllLeaveRequests()
    {
        return await _context.LeaveRequests
            .Include(x => x.Approvals)
            .ToListAsync();
    }

    public async Task<LeaveRequest?> GetLeaveRequestById(int id)
    {
        return await _context.LeaveRequests
            .Include(x => x.Approvals)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ApproveLeave(int leaveRequestId, int approverId)
    {
        var leaveRequest = await _context.LeaveRequests
            .Include(x => x.Approvals)
            .FirstOrDefaultAsync(x => x.Id == leaveRequestId);

        if (leaveRequest == null)
            return false;

        var approval = new LeaveApproval
        {
            LeaveRequestId = leaveRequestId,
            ApproverId = approverId,
            Action = "Approved"
        };

        await _context.LeaveApprovals.AddAsync(approval);

        var totalApprovals = leaveRequest.Approvals.Count + 1;

        if (totalApprovals == 1)
        {
            leaveRequest.Status = "Processing";
        }
        else if (totalApprovals >= 2)
        {
            leaveRequest.Status = "Approved";
        }

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RejectLeave(int leaveRequestId, int approverId, string reason)
    {
        var leaveRequest = await _context.LeaveRequests
            .FirstOrDefaultAsync(x => x.Id == leaveRequestId);

        if (leaveRequest == null)
            return false;

        var rejection = new LeaveApproval
        {
            LeaveRequestId = leaveRequestId,
            ApproverId = approverId,
            Action = "Rejected",
            Reason = reason
        };

        await _context.LeaveApprovals.AddAsync(rejection);

        leaveRequest.Status = "Rejected";

        await _context.SaveChangesAsync();

        return true;
    }
}