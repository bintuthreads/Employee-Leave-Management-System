using Employee_Leave_Management_System.Model.DTOs.Requests;
using FluentValidation;

namespace Employee_Leave_Management_System.Validators;

public class UpdateEmployeeRequestValidator: AbstractValidator<UpdateEmployeeRequestDto>
{
    public UpdateEmployeeRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full Name is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Enter a valid email");

        RuleFor(x => x.Department)
            .NotEmpty()
            .WithMessage("Department is required");
    }
}