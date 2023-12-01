using MassTransit;
using Models;
using PopulationApi.Domain;
using PopulationApi.Repository.Interfaces;
using RandomString4Net;
using Serilog;

namespace PopulationApi.Infrastructure.Consumers;

public class PlanetCreatedConsumer : IConsumer<PlanetCreated>
{
    private readonly ICountryRepository _countryRepository;
    private readonly IHumanRepository _humanRepository;
    private readonly Random random = new Random();

    public PlanetCreatedConsumer(ICountryRepository countryRepository, IHumanRepository humanRepository)
    {
        _countryRepository = countryRepository;
        _humanRepository = humanRepository;
    }

    // Gets the message from the rabbit queue and create country and some people
    public async Task Consume(ConsumeContext<PlanetCreated> context)
    {
        var country = await CreateCountryAsync(context.Message.Id);
        var people = await CreatePeopleAsync(country.Id);

        Log.Information($"Country: {country.Name} with {people.Count} people created for planet: {context.Message.Id}");
    }

    private async Task<CountryDomainModel> CreateCountryAsync(Guid planetId)
    {
        var countryModel = new CountryDomainModel()
        {
            Name = RandomString.GetString(Types.ALPHANUMERIC_UPPERCASE, 10),
            Population = random.Next(int.MaxValue),
            Square = random.Next(int.MaxValue / 2),
            PlanetId = planetId
        };

        return await _countryRepository.CreateAsync(countryModel);
    }

    private async Task<List<HumanDomainModel>> CreatePeopleAsync (Guid countryId)
    {
        var populationAmount = random.Next(0, 500);
        
        var people = new List<HumanDomainModel>(populationAmount);
        while (people.Count < populationAmount)
        {
            people.Add(CreateHuman(countryId));
        }

        return await _humanRepository.CreateRangeAsync(people);
    }

    private HumanDomainModel CreateHuman(Guid countryId)
    {
        var age = random.Next(0, 70);
        var gender = age > 50 ? "Male" : "Female";
        return new HumanDomainModel
        {
            Age = age,
            Gender = gender,
            Name = RandomString.GetString(Types.ALPHABET_UPPERCASE, 10),
            Surname = RandomString.GetString(Types.ALPHABET_UPPERCASE, 10),
            CountryId = countryId
        };
    }
}
