namespace PopulationApi.Repository.Entities;

public class Human
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? CountryId {  get; set; }
    public Country? Country { get; set; }
}
