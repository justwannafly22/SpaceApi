using AutoMapper;
using PlanetApi.Boundary;
using PlanetApi.Domain;

namespace PlanetApi.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PlanetRequestModel, PlanetDomainModel>();

        CreateMap<PlanetDomainModel, PlanetResponseModel>();
    }
}
