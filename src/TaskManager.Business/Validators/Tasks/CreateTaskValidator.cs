using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.DTOs.Tasks;

namespace TaskManager.Business.Validators.Tasks
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority value.");
            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Due date cannot be in the past.")
                .When(x => x.DueDate.HasValue);
        }
    }
}
