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
        var populationAmount = 3;//random.Next(0, 500);
        var people = new List<HumanDomainModel>(populationAmount);
        
        var tasks = new List<Task>(populationAmount);
        while (people.Count < populationAmount)
        {
            async Task Task()
            {
                var human = await CreateHumanAsync(countryId);
                people!.Add(human);
            }

            tasks.Add(Task());
        }

        await Task.WhenAll(tasks);

        return people;
    }

    private async Task<HumanDomainModel> CreateHumanAsync(Guid countryId)
    {
        var gender = (random.Next(0, 100) / 2) > 50 ? "Male" : "Female";
        var human = new HumanDomainModel
        {
            Age = random.Next(0, 70),
            Gender = gender,
            Name = RandomString.GetString(Types.ALPHABET_UPPERCASE, 10),
            Surname = RandomString.GetString(Types.ALPHABET_UPPERCASE, 10),
            CountryId = countryId
        };

        return await _humanRepository.CreateAsync(human);
    }
}
