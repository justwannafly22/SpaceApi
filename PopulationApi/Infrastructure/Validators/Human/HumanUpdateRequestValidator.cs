using FluentValidation;
using PopulationApi.Boundary.Human.RequestModel;

namespace PopulationApi.Infrastructure.Validators.Human;

public class HumanUpdateRequestModelValidator : AbstractValidator<HumanUpdateRequestModel>
{
    public HumanUpdateRequestModelValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 60).WithMessage("Max length for {PropertyName} is 60 characters.");

        RuleFor(r => r.Surname)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 60).WithMessage("Max length for {PropertyName} is 60 characters.");

        RuleFor(r => r.Gender)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 30).WithMessage("Max length for {PropertyName} is 30 characters.");

        RuleFor(r => r.Age)
            .GreaterThan(0).LessThan(int.MaxValue)
            .WithMessage("{PropertyName} is required and it can`t be lower than 0 or greater than 150.");
    }
}
