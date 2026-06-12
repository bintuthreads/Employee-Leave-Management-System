namespace Employee_Leave_Management_System.Model.DTOs.Requests;

public class UpdateEmployeeRequestDto
{
    public string FullName { get; set; }

    public string Email { get; set; }

    public Department Department { get; set; }
}