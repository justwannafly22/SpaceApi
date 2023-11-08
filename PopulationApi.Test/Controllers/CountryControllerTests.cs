using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Boundary.Country;
using MinimalApi.Controllers;
using MinimalApi.Domain;
using MinimalApi.Infrastructure.Exceptions;
using MinimalApi.Infrastructure.Logic.Interfaces;
using System.Net;

namespace PopulationApi.Test.Controllers;

public class CountryControllerTests
{
    private readonly CountryController _countryController;
    private readonly Mock<ICountryBusinessLogic> _mockCountryBusinessLogic;
    private readonly Mock<IMapper> _mockMapper;
    private static readonly Fixture Fixture = new();
    private readonly Guid Id = Guid.NewGuid();
    private const int _countryCount = 5;

    public CountryControllerTests()
    {
        _mockCountryBusinessLogic = new Mock<ICountryBusinessLogic>();
        _mockMapper = new Mock<IMapper>();
        _countryController = new CountryController(_mockCountryBusinessLogic.Object, _mockMapper.Object);
    }

    private static List<CountryDomainModel> TestCountries()
    {
        return Fixture.CreateMany<CountryDomainModel>(_countryCount).ToList();
    }

    private static CountryResponseModel ConvertToResponseCountry(CountryDomainModel domainModel)
    {
        return new CountryResponseModel
        {
            Id = domainModel.Id,
            Name = domainModel.Name,
            Population = domainModel.Population,
            Square = domainModel.Square
        };
    }

    private static List<CountryResponseModel> ConvertToResponseCountries(List<CountryDomainModel> countries)
    {
        var responseModels = new List<CountryResponseModel>(_countryCount);
        countries.ForEach(country =>
        {
            var responseModel = new CountryResponseModel
            {
                Id = country.Id,
                Name = country.Name,
                Population = country.Population,
                Square = country.Square
            };

            responseModels.Add(responseModel);
        });

        return responseModels;
    }

    #region GetAll
    [Fact]
    public async Task GetAll_ThereAreCountries_ReturnsCountries()
    {
        // Arrange
        var tempCountries = TestCountries();
        var responseModels = ConvertToResponseCountries(tempCountries);
        _mockCountryBusinessLogic.Setup(_ => _.GetAllCountriesAsync()).ReturnsAsync(tempCountries);
        _mockMapper.Setup(_ => _.Map<List<CountryResponseModel>>(It.IsAny<List<CountryDomainModel>>())).Returns(responseModels);

        // Act
        var response = await _countryController.GetAll();

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult!.Value.Should().NotBeNull();

            var responseValue = okObjectResult.Value;
            var countries = responseValue as List<CountryResponseModel>;

            countries.Should().NotBeNull();
            countries!.Count.Should().Be(_countryCount);
            countries.Should().BeEquivalentTo(responseModels);
        }
    }

    [Fact]
    public async Task GetAll_ThereAreNoCountries_ReturnsEmptyList()
    {
        // Arrange
        var tempCountries = new List<CountryDomainModel>();
        var responseModels = ConvertToResponseCountries(tempCountries);
        _mockCountryBusinessLogic.Setup(_ => _.GetAllCountriesAsync()).ReturnsAsync(tempCountries);
        _mockMapper.Setup(_ => _.Map<List<CountryResponseModel>>(It.IsAny<List<CountryDomainModel>>())).Returns(responseModels);

        // Act
        var response = await _countryController.GetAll();

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult!.Value.Should().NotBeNull();

            var responseValue = okObjectResult.Value;
            var countries = responseValue as List<CountryResponseModel>;

            countries.Should().NotBeNull();
            countries!.Count.Should().Be(0);
            countries.Should().BeEquivalentTo(responseModels);
        }
    }
    #endregion GetAll

    #region GetById
    [Fact]
    public async Task GetById_ThereIsCountry_ReturnsCountry()
    {
        // Arrange
        var domainModel = Fixture.Create<CountryDomainModel>();
        var responseModel = ConvertToResponseCountry(domainModel);
        _mockCountryBusinessLogic.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(domainModel);
        _mockMapper.Setup(_ => _.Map<CountryResponseModel>(It.IsAny<CountryDomainModel>())).Returns(responseModel);

        // Act
        var response = await _countryController.GetById(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult!.Value.Should().NotBeNull();

            var responseValue = okObjectResult.Value;
            var county = responseValue as CountryResponseModel;

            county.Should().NotBeNull();
            county.Should().BeEquivalentTo(responseModel);
        }
    }

    [Fact]
    public async Task GetById_ThereIsNoCountry_ReturnsNotFound()
    {
        // Arrange
        _mockCountryBusinessLogic.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var response = await _countryController.GetById(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var notFoundObjectResult = response as NotFoundObjectResult;
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            notFoundObjectResult!.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeEquivalentTo(new Tuple<string, HttpStatusCode>($"Country with id: {Id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }
    #endregion GetById

    #region Delete
    [Fact]
    public async Task Delete_ThereIsCountry_ReturnsNoContent()
    {
        // Arrange
        _mockCountryBusinessLogic.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

        // Act
        var response = await _countryController.Delete(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var noContentResult = response as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }
    }

    [Fact]
    public async Task Delete_ThereIsNoCountry_ReturnsNotFound()
    {
        // Arrange
        _mockCountryBusinessLogic.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var response = await _countryController.Delete(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var notFoundObjectResult = response as NotFoundObjectResult;
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            notFoundObjectResult!.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeEquivalentTo(new Tuple<string, HttpStatusCode>($"Country with id: {Id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }
    #endregion


}
