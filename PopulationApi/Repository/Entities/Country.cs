namespace MinimalApi.Repository.Entities;

public class Country
{
    public Guid Id { get; set; }
    public string Name {  get; set; }
    public int Population { get; set; }
    public int Square { get; set; }

    public List<Human>? People {  get; set; }
}
