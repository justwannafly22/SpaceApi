namespace PopulationApi.Boundary.Country.RequestModels;

public class CountryCreateRequestModel
{
    public string Name { get; set; }
    public int Population { get; set; }
    public int Square { get; set; }
}
