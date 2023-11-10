namespace PopulationApi.Boundary.Country.RequestModels;

public class CountryUpdateRequestModel
{
    public string Name { get; set; }
    public int Population { get; set; }
    public int Square { get; set; }
}
