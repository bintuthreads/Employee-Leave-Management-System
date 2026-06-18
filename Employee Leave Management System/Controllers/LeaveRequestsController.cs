using Employee_Leave_Management_System.Model.DTOs.Requests;
using Employee_Leave_Management_System.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/leaves")]
public class LeaveRequestsController : ControllerBase
{
    private readonly ILeaveRepository _leaveRepository;

    public LeaveRequestsController(ILeaveRepository leaveRepository)
    {
        _leaveRepository = leaveRepository;
    }

    // CREATE LEAVE
    [HttpPost]
    public async Task<IActionResult> SubmitLeave(SubmitLeaveRequestDto dto)
    {
        var result = await _leaveRepository.SubmitLeave(dto);
        return Ok(result);
    }

    // GET ALL LEAVES
    [HttpGet]
    public async Task<IActionResult> GetAllLeaves()
    {
        var leaves = await _leaveRepository.GetAllLeaves();
        return Ok(leaves);
    }

    // GET BY ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLeaveById(int id)
    {
        var leave = await _leaveRepository.GetLeaveById(id);
        return Ok(leave);
    }

    // UPDATE
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateLeave(int id, SubmitLeaveRequestDto dto)
    {
        var result = await _leaveRepository.UpdateLeave(id, dto);
        return Ok(result);
    }

    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLeave(int id)
    {
        var result = await _leaveRepository.DeleteLeave(id);
        return Ok(result);
    }

    // APPROVE
    [HttpPost("approve/{id:int}")]
    public async Task<IActionResult> ApproveLeave(int id, LeaveActionRequestDto dto)
    {
        var result = await _leaveRepository.ApproveLeave(id, dto);
        return Ok(result);
    }

    // REJECT
    [HttpPost("reject/{id:int}")]
    public async Task<IActionResult> RejectLeave(int id, LeaveActionRequestDto dto)
    {
        var result = await _leaveRepository.RejectLeave(id, dto);
        return Ok(result);
    }

    // STATUS FILTER
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var result = await _leaveRepository.GetLeavesByStatus(status);
        return Ok(result);
    }

    // EMPLOYEES ON LEAVE
    [HttpGet("employees/on-leave")]
    public async Task<IActionResult> GetEmployeesCurrentlyOnLeave()
    {
        var result = await _leaveRepository.GetEmployeesCurrentlyOnLeave();
        return Ok(result);
    }

    // STATISTICS
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var result = await _leaveRepository.GetLeaveStatistics();
        if (result == null)
            return Ok(new { total = 0 });
        return Ok(result);
    }
}