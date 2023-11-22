using AutoMapper;
using IdentityApi.Boundary;
using IdentityApi.Domain;

namespace IdentityApi.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginRequestModel, LoginDomainModel>();
        CreateMap<RegisterRequestModel, RegisterDomainModel>();
    }
}
