using Employee_Leave_Management_System.Model.DTOs.Requests;
using Employee_Leave_Management_System.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Leave_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestsController : ControllerBase
{
    private readonly ILeaveRepository _leaveRepository;
    public LeaveRequestsController(ILeaveRepository leaveRepository)
    {
        _leaveRepository = leaveRepository;
    }
    
    //Submit Leave
    [HttpPost("Submit-Leave")]
    public async Task<IActionResult> SubmitLeave(SubmitLeaveRequestDto dto)
    {
        var result = await _leaveRepository.SubmitLeave(dto);
        return Ok(result);
    }
    
    //Update Leave
    [HttpPut("Update-Leave/{id}")]
    public async Task<IActionResult> UpdateLeave(int id, SubmitLeaveRequestDto dto)
    {
        var result = await _leaveRepository.UpdateLeave(id, dto);

        return Ok(result);
    }
    
    //Delete Leave
    [HttpDelete("Delete-Leave/{id}")]
    public async Task<IActionResult> DeleteLeave(int id)
    {
        var result = await _leaveRepository.DeleteLeave(id);

        return Ok(result);
    }
    
    // Get All Leaves
    [HttpGet("Get-All-Leaves")]
    public async Task<IActionResult> GetAllLeaves()
    {
        var leaves = await _leaveRepository.GetAllLeaves();
        return Ok(leaves);
    }
    
    // Get Leave By Id
    [HttpGet("Get-Leave-by-Id{id}")]
    public async Task<IActionResult> GetLeaveById(int id)
    {
        var leave = await _leaveRepository.GetLeaveById(id);
        return Ok(leave);
    }
    
    //Approve Leave
    [HttpPost("Approve-Leave/{id}")]
    public async Task<IActionResult> ApproveLeave(int id, LeaveActionRequestDto dto)
    {
        var result = await _leaveRepository.ApproveLeave(id, dto);
        return Ok(result);
    }
    
    //Reject Leave
    [HttpPost("Reject-Leave/{id}")]
    public async Task<IActionResult> RejectLeave(int id, LeaveActionRequestDto dto)
    {
        var result = await _leaveRepository.RejectLeave(id, dto);
        return Ok(result);
    }
    
    //Leave Request by Status
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var result = await _leaveRepository.GetLeavesByStatus(status);
        return Ok(result);
    }
    
    // Get Employees Currently on Leave
    [HttpGet("Current-Employees-On-Leave")]
    public async Task<IActionResult> GetEmployeesCurrentlyOnLeave()
    {
        var result = await _leaveRepository.GetEmployeesCurrentlyOnLeave();
        return Ok(result);
    }
    
    // Get leave statistics
    [HttpGet("Leave-Statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var result = await _leaveRepository.GetLeaveStatistics();
        return Ok(result);
    }
}