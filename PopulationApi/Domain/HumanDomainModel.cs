namespace MinimalApi.Domain;

public class HumanDomainModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public Guid? CountryId { get; set; }
}
