using FluentValidation;
using PlanetApi.Boundary;

namespace PlanetApi.Infrastructure.Validators.Planet;

public class PlanetCreateRequestModelValidator : AbstractValidator<PlanetRequestModel>
{
    public PlanetCreateRequestModelValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 50).WithMessage("Max length for {PropertyName} is 60 characters.");
        
        RuleFor(r => r.Location)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 50).WithMessage("Max length for {PropertyName} is 60 characters.");
        
        RuleFor(r => r.Air)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 50).WithMessage("Max length for {PropertyName} is 60 characters.");

        RuleFor(r => r.Age)
            .GreaterThan(0).LessThan(long.MaxValue)
            .WithMessage("{PropertyName} is required and it can`t be lower than 0.");
    }
}
