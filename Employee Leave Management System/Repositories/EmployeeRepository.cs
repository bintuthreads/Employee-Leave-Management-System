using Employee_Leave_Management_System.Data;
using Employee_Leave_Management_System.Model;
using Employee_Leave_Management_System.Model.DTOs;
using Employee_Leave_Management_System.Model.DTOs.Requests;
using Employee_Leave_Management_System.Model.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace Employee_Leave_Management_System.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbcontext;

    public EmployeeRepository(ApplicationDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    //GET ALL EMPLOYEES
    public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployees()
    {
        return await _dbcontext.Employees
            .Select(e => new EmployeeResponseDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                Department = e.Department
            })
            .ToListAsync();
    }

    //GET EMPLOYEE BY ID
    public async Task<EmployeeResponseDto?> GetEmployeeById(int id)
    {
        return await _dbcontext.Employees
            .Where(e => e.Id == id)
            .Select(e => new EmployeeResponseDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                Department = e.Department
            })
            .FirstOrDefaultAsync();
        
    }

    //CREATE EMPLOYEE
    public async Task<Employee> CreateEmployee(CreateEmployeeRequestDto dto)
    {
        var employeeExists = await _dbcontext.Employees
            .AnyAsync(e =>
                e.FullName == dto.FullName ||
                e.Email == dto.Email);

        if (employeeExists)
            throw new Exception("Employee already exists");

        if (!Departments.ValidDepartments.Contains(dto.Department))
            throw new Exception("Invalid department");

        var employee = new Employee
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Department = dto.Department,
            DateJoined = DateTime.UtcNow
        };

        await _dbcontext.Employees.AddAsync(employee);
        await _dbcontext.SaveChangesAsync();

        return employee;
    }

    // UPDATE EMPLOYEE
    public async Task<string> UpdateEmployee(int id, UpdateEmployeeRequestDto dto)
    {
        var employee = await _dbcontext.Employees.FindAsync(id);
        if (employee == null)
            return "Employee not found";
        employee.FullName = dto.FullName;
        employee.Email = dto.Email;
        employee.Department = dto.Department;

        await _dbcontext.SaveChangesAsync();
        return "Employee updated successfully";
    }

    //DELETE EMPLOYEE
    public async Task<string> DeleteEmployee(int id)
    {
        var employee = await _dbcontext.Employees.FindAsync(id);

        if (employee == null)
            return "Employee not found";

        _dbcontext.Employees.Remove(employee);
        await _dbcontext.SaveChangesAsync();

        return "Employee deleted successfully";
    }
    
    //GET EMPLOYEE LEAVES
    public async Task<IEnumerable<LeaveRequestResponseDto>> GetEmployeeLeaves(int employeeId)
    {
        return await _dbcontext.LeaveRequests
            .Where(l => l.EmployeeId == employeeId)
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
}