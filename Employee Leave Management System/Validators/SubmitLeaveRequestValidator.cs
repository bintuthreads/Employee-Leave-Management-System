using Employee_Leave_Management_System.Model.DTOs.Requests;
using FluentValidation;

namespace Employee_Leave_Management_System.Validators;

public class SubmitLeaveRequestValidator : AbstractValidator<SubmitLeaveRequestDto>
{
    public SubmitLeaveRequestValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0);

        RuleFor(x => x.Reason)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start date cannot be after End date");
    }
}