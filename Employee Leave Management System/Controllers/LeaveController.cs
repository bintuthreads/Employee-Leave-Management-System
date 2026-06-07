using Employee_Leave_Management_System.Model;
using Employee_Leave_Management_System.Model.DTOs;
using Employee_Leave_Management_System.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace Employee_Leave_Management_System.Controllers;


[ApiController]
[Route("api/[controller]")]
public class LeavesController : ControllerBase
{
    private readonly ILeaveRepository _repository;
    public LeavesController(ILeaveRepository repository)
    {
        _repository = repository;
    }
    
    //GET ALL LEAVES
    [HttpGet("Get-All-Leaves")]
    public async Task<IActionResult> GetAllLeaveRequests()
    {
        var leaveRequests = await _repository.GetAllLeaveRequests();
        return Ok(leaveRequests);
    }
    
    //GET LEAVE BY ID
    [HttpGet("Get-Leave-by-ID/{id}")]
    public async Task<IActionResult> GetLeaveRequestById(int id)
    {
        var leaveRequest = await _repository.GetLeaveRequestById(id);
        return Ok(leaveRequest);
    }
    
    //SUBMIT LEAVE
    [HttpPost("Submit-Leave")]
    public async Task<IActionResult> SubmitLeaveRequest(LeaveRequestDto leaveRequestDto)
    {
        var leaveRequest = await _repository.SubmitLeaveRequest(leaveRequestDto);
        return Ok(leaveRequest);
    }
    
    //UPDATE LEAVE
    [HttpPut("Update-Leave/{id}")]
    public async Task<IActionResult> UpdateLeaveRequest(int id, LeaveRequestDto leaveRequestDto)
    {
        var leaveRequest = await _repository.UpdateLeaveRequest(id, leaveRequestDto);
        return Ok(leaveRequest);
    }
    
    //DELETE LEAVE
    [HttpDelete("Delete-Leave/{id}")]
    public async Task<IActionResult> DeleteLeaveRequest(int id)
    {
        var leaveRequest = await _repository.DeleteLeaveRequest(id);
        return Ok(leaveRequest);
    }
    
    //APPROVE LEAVE
    [HttpPut("Approve-Leave/{id}")]
    public async Task<IActionResult> ApproveLeaveRequest(int id)
    {
        var leaveRequest = await _repository.ApproveLeaveRequest(id);
        return Ok(leaveRequest);
    }
    
    //REJECT LEAVE
    [HttpPut("Reject-Leave/{id}")]
    public async Task<IActionResult> RejectLeaveRequest(int id, LeaveRejectDto leaveRejectDto)
    {
        var leaveRequest = await _repository.RejectLeaveRequest(id, leaveRejectDto);
        return Ok(leaveRequest);
    }
    
    //GET LEAVE BY STATUS
    [HttpGet("Get-Leaves-by-Status/{status}")]
    public async Task<IActionResult> GetLeaveByStatus(string status)
    {
        var leaves = await _repository.GetLeaveByStatus(status);
        return Ok(leaves);
    }
    
    // GET LEAVE STATISTICS BY DEPARTMENT
    [HttpGet("Leave-Statistic-by-Department/{department}")]
    public async Task<IActionResult> GetDepartmentByDepartment(string department)
    {
        var leaves = await _repository.GetLeavesStatsByDepartment(department);
        return Ok(leaves);
    }
    
    //GET EMPLOYEES CURRENTLY ON LEAVE
    [HttpGet("Get-Employees-Currently-On-Leave")]
    public async Task<IActionResult> GetEmployeesOnLeave()
    {
        var leaves = await _repository.GetEmployeesOnLeave();
        return Ok(leaves);
    }
}