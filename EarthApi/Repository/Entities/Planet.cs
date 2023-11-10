namespace PlanetApi.Repository.Entities;

public class Planet
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public long Age { get; set; }
    public string Air { get; set; }
}
