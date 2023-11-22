using IdentityApi.Domain;

namespace IdentityApi.Infrastructure.Logic;

public interface IIdentityLogic
{
    public Task<TokenDomainModel> LoginAsync(LoginDomainModel domainModel);
}
