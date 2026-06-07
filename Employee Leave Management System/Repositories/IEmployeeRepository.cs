using Employee_Leave_Management_System.Model;
using Employee_Leave_Management_System.Model.DTOs;

namespace Employee_Leave_Management_System.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployees();

    Task<Employee?> GetEmployeeById(int id);

    Task<Employee> CreateEmployee(CreateEmployeeDto createEmployeeDto);

    Task<Employee?> UpdateEmployee(int id, CreateEmployeeDto createEmployeeDto);

    Task<string> DeleteEmployee(int id);

    Task<IEnumerable<LeaveRequest>> GetEmployeeLeavesHistory(int employeeId);
}