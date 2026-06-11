using Employee_Leave_Management_System.Model;
using Employee_Leave_Management_System.Model.DTOs.Requests;
using FluentValidation;

namespace Employee_Leave_Management_System.Validators;
public class LeaveActionRequestValidator : AbstractValidator<LeaveActionRequestDto>
{
    public LeaveActionRequestValidator()
    {
        RuleFor(x => x.ApproverId)
            .GreaterThan(0)
            .WithMessage("Approver Id must be greater than 0");
    }
}