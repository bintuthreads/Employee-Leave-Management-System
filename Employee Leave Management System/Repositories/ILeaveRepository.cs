using Employee_Leave_Management_System.Model;
using Employee_Leave_Management_System.Model.DTOs;
using Employee_Leave_Management_System.Model.DTOs.Requests;
using Employee_Leave_Management_System.Model.DTOs.Responses;

namespace Employee_Leave_Management_System.Repositories;

public interface ILeaveRepository
{
    Task<IEnumerable<LeaveRequestResponseDto>> GetAllLeaves();

    Task<LeaveRequestResponseDto?> GetLeaveById(int id);

    Task<string> SubmitLeave(SubmitLeaveRequestDto dto);

    Task<string> UpdateLeave(int id, SubmitLeaveRequestDto dto);

    Task<string> DeleteLeave(int id);

    Task<string> ApproveLeave(int id, LeaveActionRequestDto dto);

    Task<string> RejectLeave(int id, LeaveActionRequestDto dto);

    Task<IEnumerable<LeaveRequestResponseDto>> GetLeavesByStatus(string status);

    Task<IEnumerable<EmployeeResponseDto>> GetEmployeesCurrentlyOnLeave();

    Task<object> GetLeaveStatistics();
}