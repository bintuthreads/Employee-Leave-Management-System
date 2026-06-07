using Employee_Leave_Management_System.Model;
using Employee_Leave_Management_System.Model.DTOs;
using Employee_Leave_Management_System.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace Employee_Leave_Management_System.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repository;
    public EmployeesController(IEmployeeRepository repository)
    {
        _repository = repository;
    }
    
    
    //GET ALL EMPLOYEES
    [HttpGet("Get-All-Employees")]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _repository.GetAllEmployees();
        return Ok(employees);
    }
    
    //GET EMPLOYEE BY ID
    [HttpGet("Get-Employee-by-ID/{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await _repository.GetEmployeeById(id);
        return Ok(employee);
    }
    
    //CREATE EMPLOYEE
    [HttpPost("Create-Employee")]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeDto createEmployeeDto)
    {
        var employee = await  _repository.CreateEmployee(createEmployeeDto);
        return Ok(employee);
    }
    
    //UPDATE EMPLOYEE
    [HttpPut("Update-Employee/{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, CreateEmployeeDto createEmployeeDto)
    {
        var employee = await _repository.UpdateEmployee(id, createEmployeeDto);
        return Ok(employee);
    }
    
    //DELETE EMPLOYEE
    [HttpDelete("Delete-Employee/{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var result = await _repository.DeleteEmployee(id);
        return Ok(result);
    }
    
    //GET EMPLOYEE LEAVES
    [HttpGet("Get-Employee-Leaves/{employeeId}")]
    public async Task<IActionResult> GetEmployeeLeaves(int employeeId)
    {
        var leaves = await _repository.GetEmployeeLeavesHistory(employeeId);
        return Ok(leaves);
    }
}