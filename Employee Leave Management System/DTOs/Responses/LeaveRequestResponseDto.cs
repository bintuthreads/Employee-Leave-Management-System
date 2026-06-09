using Employee_Leave_Management_System.Model.DTOs.Responses;

namespace Employee_Leave_Management_System.Model.DTOs;

public class LeaveRequestResponseDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string LeaveType { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Reason { get; set; }

    public string Status { get; set; }

    public DateTime DateCreated { get; set; }

    public List<LeaveApprovalResponseDto> Approvals { get; set; } = new();
}