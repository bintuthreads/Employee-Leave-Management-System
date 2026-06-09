using Employee_Leave_Management_System.Model;

namespace Employee_Leave_Management_System.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllEmployees();

    Task<Employee?> GetEmployeeById(int id);

    Task<Employee> CreateEmployee(Employee employee);

    Task<Employee?> UpdateEmployee(int id, Employee employee);

    Task<bool> DeleteEmployee(int id);
}