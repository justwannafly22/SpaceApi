using FluentValidation;
using PopulationApi.Boundary.Country.RequestModels;

namespace PopulationApi.Infrastructure.Validators.Country;

public class CountryUpdateRequestModelValidator : AbstractValidator<CountryUpdateRequestModel>
{
    public CountryUpdateRequestModelValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 60).WithMessage("Max length for {PropertyName} is 60 characters.");

        RuleFor(r => r.Square)
            .GreaterThan(0).LessThan(int.MaxValue)
            .WithMessage("{PropertyName} is required and it can`t be lower than 0.");

        RuleFor(r => r.Population)
            .GreaterThan(0).LessThan(int.MaxValue)
            .WithMessage("{PropertyName} is required and it can`t be lower than 0.");
    }
}
