using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.DTOs.Users;

namespace TaskManager.Business.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .MaximumLength(100).WithMessage("Name cannot exceed 50 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Email is invalid.");
        }
    }
}
