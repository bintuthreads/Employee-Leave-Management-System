using Employee_Leave_Management_System.Model;
using Employee_Leave_Management_System.Model.DTOs;
using Employee_Leave_Management_System.Model.DTOs.Requests;
using Employee_Leave_Management_System.Model.DTOs.Responses;

namespace Employee_Leave_Management_System.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeResponseDto>> GetAllEmployees();

    Task<EmployeeResponseDto?> GetEmployeeById(int id);

    Task<Employee> CreateEmployee(CreateEmployeeRequestDto dto);

    Task<string> UpdateEmployee(int id, UpdateEmployeeRequestDto dto);

    Task<string> DeleteEmployee(int id);

    Task<IEnumerable<LeaveRequestResponseDto>> GetEmployeeLeaves(int employeeId);
}