using Employee_Leave_Management_System.Model.DTOs.Requests;
using Employee_Leave_Management_System.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Leave_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    //Get All Employees
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _employeeRepository.GetAllEmployees();

        return Ok(employees);
    }
    
    //Get Employee By Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        return Ok(await _employeeRepository.GetEmployeeById(id));
    }

    
    //Create Employee
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeRequestDto dto)
    {
        var employee = await _employeeRepository.CreateEmployee(dto);
        return Ok(employee);
    }
    
    //Update Employee
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(
        int id,
        UpdateEmployeeRequestDto dto)
    {
        var result = await _employeeRepository.UpdateEmployee(id, dto);

        return Ok(result);
    }
    
    // Delete Employee
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var result = await _employeeRepository.DeleteEmployee(id);

        return Ok(result);
    }
    
    // Get Employee Leaves
    [HttpGet("{id}/leaves")]
    public async Task<IActionResult> GetEmployeeLeaves(int id)
    {
        return Ok(await _employeeRepository.GetEmployeeLeaves(id));
    }
}