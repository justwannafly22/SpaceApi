namespace IdentityApi.Domain;

public class TokenDomainModel
{
    public string TokenString { get; set; }
    public DateTime Expiration { get; set; }
}
