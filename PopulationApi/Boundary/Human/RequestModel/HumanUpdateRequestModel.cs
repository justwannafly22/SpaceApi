namespace PopulationApi.Boundary.Human.RequestModel;

public class HumanUpdateRequestModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public Guid CountryId { get; set; }
}
