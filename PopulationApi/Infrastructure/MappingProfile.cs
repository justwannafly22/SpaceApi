using AutoMapper;
using PopulationApi.Boundary.Country;
using PopulationApi.Boundary.Country.RequestModels;
using PopulationApi.Boundary.Human;
using PopulationApi.Boundary.Human.RequestModel;
using PopulationApi.Domain;

namespace PopulationApi.Infrastructure;

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
