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
    [HttpPost]
    public async Task<IActionResult> SubmitLeave(SubmitLeaveRequestDto dto)
    {
        var result = await _leaveRepository.SubmitLeave(dto);
        return Ok(result);
    }
    
    // Get All Leaves
    [HttpGet]
    public async Task<IActionResult> GetAllLeaves()
    {
        var leaves = await _leaveRepository.GetAllLeaves();
        return Ok(leaves);
    }
    
    // Get Leave By Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeaveById(int id)
    {
        var leave = await _leaveRepository.GetLeaveById(id);
        if (leave == null)
            return NotFound();
        return Ok(leave);
    }
    
    //Approve Leave
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveLeave(int id, LeaveActionRequestDto dto)
    {
        var result = await _leaveRepository.ApproveLeave(id, dto);
        return Ok(result);
    }
    
    //Reject Leave
    [HttpPost("{id}/reject")]
    public async Task<IActionResult> RejectLeave(int id, LeaveActionRequestDto dto)
    {
        var result = await _leaveRepository.RejectLeave(id, dto);
        return Ok(result);
    }
}