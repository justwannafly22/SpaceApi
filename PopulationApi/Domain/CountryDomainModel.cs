namespace PopulationApi.Domain;

public class CountryDomainModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Population { get; set; }
    public int Square { get; set; }
    public Guid PlanetId { get; set; }
}
