using Employee_Leave_Management_System.Model;

namespace Employee_Leave_Management_System.Repositories;

public interface ILeaveRepository
{
    Task<LeaveRequest> SubmitLeaveRequest(LeaveRequest leaveRequest);

    Task<List<LeaveRequest>> GetAllLeaveRequests();

    Task<LeaveRequest?> GetLeaveRequestById(int id);

    Task<bool> ApproveLeave(int leaveRequestId, int approverId);

    Task<bool> RejectLeave(int leaveRequestId, int approverId, string reason);
}