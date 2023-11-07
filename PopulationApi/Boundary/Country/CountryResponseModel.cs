namespace MinimalApi.Boundary.Country;

public class CountryResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Population { get; set; }
    public int Square { get; set; }
}
