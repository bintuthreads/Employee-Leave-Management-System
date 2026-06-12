namespace Employee_Leave_Management_System.Model.DTOs.Responses;

public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public Department Department { get; set; }

    public DateTime DateJoined { get; set; }
}