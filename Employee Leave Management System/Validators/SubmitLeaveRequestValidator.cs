using Employee_Leave_Management_System.Model.DTOs.Requests;
using FluentValidation;

namespace Employee_Leave_Management_System.Validators;

public class SubmitLeaveRequestValidator : AbstractValidator<SubmitLeaveRequestDto>
{
    public SubmitLeaveRequestValidator()
    {
        RuleFor(x => x.EmployeeId).GreaterThan(0);

        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End Date must be after Start Date");
    }
}