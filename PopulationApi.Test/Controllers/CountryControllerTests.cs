using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PopulationApi.Boundary.Country;
using PopulationApi.Boundary.Country.RequestModels;
using PopulationApi.Controllers;
using PopulationApi.Domain;
using PopulationApi.Infrastructure.Exceptions;
using PopulationApi.Infrastructure.Logic.Interfaces;
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

    public static List<CountryDomainModel> TestCountries()
    {
        return Fixture.CreateMany<CountryDomainModel>(_countryCount).ToList();
    }

    #region GetAll
    [Fact]
    public async Task GetAll_ThereAreCountries_ReturnsCountries()
    {
        // Arrange
        var tempCountries = TestCountries();
        var responseModels = CountryControllerTestsExtensions.ConvertToResponseCountries(tempCountries, tempCountries.Count);
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
        var responseModels = CountryControllerTestsExtensions.ConvertToResponseCountries(tempCountries, tempCountries.Count);
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
        var responseModel = CountryControllerTestsExtensions.ConvertToResponseCountry(domainModel);
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

    #region Create
    [Fact]
    public async Task Create_ValidModel_ReturnsCountry()
    {
        // Arrange
        var requestModel = Fixture.Create<CountryCreateRequestModel>();
        var domainModel = CountryControllerTestsExtensions.ConvertToDomainModel(requestModel);
        var responseModel = CountryControllerTestsExtensions.ConvertToResponseCountry(domainModel);
        _mockCountryBusinessLogic.Setup(_ => _.CreateAsync(It.IsAny<CountryDomainModel>())).ReturnsAsync(domainModel);
        _mockMapper.Setup(_ => _.Map<CountryResponseModel>(It.IsAny<CountryDomainModel>())).Returns(responseModel);
        _mockMapper.Setup(_ => _.Map<CountryDomainModel>(It.IsAny<CountryCreateRequestModel>())).Returns(domainModel);

        // Act
        var response = await _countryController.Create(requestModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var createdAtActionResult = response as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            createdAtActionResult!.Value.Should().NotBeNull();

            var responseValue = createdAtActionResult.Value;
            var county = responseValue as CountryResponseModel;

            county.Should().NotBeNull();
            county.Should().BeEquivalentTo(responseModel);
        }
    }

    [Fact]
    public async Task Create_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var requestModel = Fixture.Create<CountryCreateRequestModel>();
        _countryController.ModelState.AddModelError("Bad request error", "Model is invalid");

        // Act
        var response = await _countryController.Create(requestModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var badRequestObjectResult = response as BadRequestObjectResult;
            badRequestObjectResult.Should().NotBeNull();
            badRequestObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            badRequestObjectResult!.Value.Should().NotBeNull();
        }
    }
    #endregion

    #region Update
    [Fact]
    public async Task Update_ValidModel_ReturnsNoContent()
    {
        // Arrange
        var requestModel = Fixture.Create<CountryUpdateRequestModel>();
        var domainModel = CountryControllerTestsExtensions.ConvertToDomainModel(requestModel);
        var responseModel = CountryControllerTestsExtensions.ConvertToResponseCountry(domainModel);
        _mockCountryBusinessLogic.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CountryDomainModel>())).ReturnsAsync(domainModel);
        _mockMapper.Setup(_ => _.Map<CountryResponseModel>(It.IsAny<CountryDomainModel>())).Returns(responseModel);
        _mockMapper.Setup(_ => _.Map<CountryDomainModel>(It.IsAny<CountryUpdateRequestModel>())).Returns(domainModel);

        // Act
        var response = await _countryController.Update(domainModel.Id, requestModel);

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
    public async Task Update_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var requestModel = Fixture.Create<CountryUpdateRequestModel>();
        _countryController.ModelState.AddModelError("Bad request error", "Model is invalid");

        // Act
        var response = await _countryController.Update(Id, requestModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var badRequestObjectResult = response as BadRequestObjectResult;
            badRequestObjectResult.Should().NotBeNull();
            badRequestObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            badRequestObjectResult!.Value.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task Update_NonExistCountry_ReturnsNotFound()
    {
        // Arrange
        var requestModel = Fixture.Create<CountryUpdateRequestModel>();
        _mockCountryBusinessLogic.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CountryDomainModel>())).ThrowsAsync(new NotFoundException());

        // Act
        var response = await _countryController.Update(Id, requestModel);

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

internal static class CountryControllerTestsExtensions
{
    public static CountryResponseModel ConvertToResponseCountry(CountryDomainModel domainModel)
    {
        return new CountryResponseModel
        {
            Id = domainModel.Id,
            Name = domainModel.Name,
            Population = domainModel.Population,
            Square = domainModel.Square
        };
    }

    public static List<CountryResponseModel> ConvertToResponseCountries(List<CountryDomainModel> countries, int count)
    {
        var responseModels = new List<CountryResponseModel>(count);
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

    public static CountryDomainModel ConvertToDomainModel(CountryCreateRequestModel requestModel)
    {
        return new CountryDomainModel
        {
            Id = Guid.NewGuid(),
            Name = requestModel.Name,
            Population = requestModel.Population,
            Square = requestModel.Square
        };
    }

    public static CountryDomainModel ConvertToDomainModel(CountryUpdateRequestModel requestModel)
    {
        return new CountryDomainModel
        {
            Id = Guid.NewGuid(),
            Name = requestModel.Name,
            Population = requestModel.Population,
            Square = requestModel.Square
        };
    }
}
