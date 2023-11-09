using AutoMapper;
using MinimalApi.Boundary.Country;
using MinimalApi.Boundary.Country.RequestModels;
using MinimalApi.Boundary.Human;
using MinimalApi.Boundary.Human.RequestModel;
using MinimalApi.Domain;

namespace MinimalApi.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Country
        CreateMap<CountryCreateRequestModel, CountryDomainModel>();
        CreateMap<CountryUpdateRequestModel, CountryDomainModel>();

        CreateMap<CountryDomainModel, CountryResponseModel>();
        #endregion

        #region Human
        CreateMap<HumanCreateRequestModel, HumanDomainModel>();
        CreateMap<HumanUpdateRequestModel, HumanDomainModel>();

        CreateMap<HumanDomainModel, HumanResponseModel>();
        #endregion
    }
}
