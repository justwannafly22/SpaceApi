using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlanetApi.Boundary;
using PlanetApi.Controllers;
using PlanetApi.Domain;
using PlanetApi.Infrastructure.Exceptions;
using PlanetApi.Infrastructure.Logic;
using System;
using System.Net;
using static System.Collections.Specialized.BitVector32;

namespace PlanetApi.Test.Controllers;

public class PlanetControllerTests
{
    private readonly PlanetController _planetController;
    private readonly Mock<IPlanetBusinessLogic> _mockPlanetBusinessLogic;
    private readonly Mock<IMapper> _mockMapper;
    private static readonly Fixture Fixture = new();
    private readonly Guid Id = Guid.NewGuid();
    private const int _planetCount = 5;

    public PlanetControllerTests()
    {
        _mockPlanetBusinessLogic = new Mock<IPlanetBusinessLogic>();
        _mockMapper = new Mock<IMapper>();
        _planetController = new PlanetController(_mockPlanetBusinessLogic.Object, _mockMapper.Object);
    }

    public static List<PlanetDomainModel> TestPlanets()
    {
        return Fixture.CreateMany<PlanetDomainModel>(_planetCount).ToList();
    }

    #region GetAll
    [Fact]
    public async Task GetAll_ThereArePlanets_ReturnsPlanets()
    {
        // Arrange
        var tempPlanets = TestPlanets();
        var responseModels = PlanetControllerTestsExtensions.ConvertToResponsePlanets(tempPlanets, tempPlanets.Count);
        _mockPlanetBusinessLogic.Setup(_ => _.GetAllPlanetsAsync()).ReturnsAsync(tempPlanets);
        _mockMapper.Setup(_ => _.Map<List<PlanetResponseModel>>(It.IsAny<List<PlanetDomainModel>>())).Returns(responseModels);

        // Act
        var response = await _planetController.GetAll();

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult!.Value.Should().NotBeNull();

            var responseValue = okObjectResult.Value;
            var planets = responseValue as List<PlanetResponseModel>;

            planets.Should().NotBeNull();
            planets!.Count.Should().Be(_planetCount);
            planets.Should().BeEquivalentTo(responseModels);
        }
    }

    [Fact]
    public async Task GetAll_ThereAreNoPlanets_ReturnsEmptyList()
    {
        // Arrange
        var tempPlanets = new List<PlanetDomainModel>();
        var responseModels = PlanetControllerTestsExtensions.ConvertToResponsePlanets(tempPlanets, tempPlanets.Count);
        _mockPlanetBusinessLogic.Setup(_ => _.GetAllPlanetsAsync()).ReturnsAsync(tempPlanets);
        _mockMapper.Setup(_ => _.Map<List<PlanetResponseModel>>(It.IsAny<List<PlanetDomainModel>>())).Returns(responseModels);

        // Act
        var response = await _planetController.GetAll();

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult!.Value.Should().NotBeNull();

            var responseValue = okObjectResult.Value;
            var planets = responseValue as List<PlanetResponseModel>;

            planets.Should().NotBeNull();
            planets!.Count.Should().Be(0);
            planets.Should().BeEquivalentTo(responseModels);
        }
    }
    #endregion GetAll

    #region GetById
    [Fact]
    public async Task GetById_ThereIsPlanet_ReturnsPlanet()
    {
        // Arrange
        var domainModel = Fixture.Create<PlanetDomainModel>();
        var responseModel = PlanetControllerTestsExtensions.ConvertToResponsePlanet(domainModel);
        _mockPlanetBusinessLogic.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(domainModel);
        _mockMapper.Setup(_ => _.Map<PlanetResponseModel>(It.IsAny<PlanetDomainModel>())).Returns(responseModel);

        // Act
        var response = await _planetController.GetById(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult!.Value.Should().NotBeNull();

            var responseValue = okObjectResult.Value;
            var county = responseValue as PlanetResponseModel;

            county.Should().NotBeNull();
            county.Should().BeEquivalentTo(responseModel);
        }
    }

    [Fact]
    public async Task GetById_ThereIsNoPlanet_ReturnsNotFound()
    {
        // Arrange
        _mockPlanetBusinessLogic.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _planetController.GetById(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<NotFoundException>(action);
        }
    }
    #endregion GetById

    #region Delete
    [Fact]
    public async Task Delete_ThereIsPlanet_ReturnsNoContent()
    {
        // Arrange
        _mockPlanetBusinessLogic.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

        // Act
        var response = await _planetController.Delete(Id);

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
    public async Task Delete_ThereIsNoPlanet_ThrowsNotFoundException()
    {
        // Arrange
        _mockPlanetBusinessLogic.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _planetController.Delete(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<NotFoundException>(action);
        }
    }
    #endregion

    #region Create
    [Fact]
    public async Task Create_ValidModel_ReturnsPlanet()
    {
        // Arrange
        var requestModel = Fixture.Create<PlanetRequestModel>();
        var domainModel = PlanetControllerTestsExtensions.ConvertToDomainModel(requestModel);
        var responseModel = PlanetControllerTestsExtensions.ConvertToResponsePlanet(domainModel);
        _mockPlanetBusinessLogic.Setup(_ => _.CreateAsync(It.IsAny<PlanetDomainModel>())).ReturnsAsync(domainModel);
        _mockMapper.Setup(_ => _.Map<PlanetResponseModel>(It.IsAny<PlanetDomainModel>())).Returns(responseModel);
        _mockMapper.Setup(_ => _.Map<PlanetDomainModel>(It.IsAny<PlanetRequestModel>())).Returns(domainModel);

        // Act
        var response = await _planetController.Create(requestModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var createdAtActionResult = response as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            createdAtActionResult!.Value.Should().NotBeNull();

            var responseValue = createdAtActionResult.Value;
            var county = responseValue as PlanetResponseModel;

            county.Should().NotBeNull();
            county.Should().BeEquivalentTo(responseModel);
        }
    }

    [Fact]
    public async Task Create_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var requestModel = Fixture.Create<PlanetRequestModel>();
        _planetController.ModelState.AddModelError("Bad request error", "Model is invalid");

        // Act
        var response = await _planetController.Create(requestModel);

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
        var requestModel = Fixture.Create<PlanetRequestModel>();
        var domainModel = PlanetControllerTestsExtensions.ConvertToDomainModel(requestModel);
        var responseModel = PlanetControllerTestsExtensions.ConvertToResponsePlanet(domainModel);
        _mockPlanetBusinessLogic.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PlanetDomainModel>())).ReturnsAsync(domainModel);
        _mockMapper.Setup(_ => _.Map<PlanetResponseModel>(It.IsAny<PlanetDomainModel>())).Returns(responseModel);
        _mockMapper.Setup(_ => _.Map<PlanetDomainModel>(It.IsAny<PlanetRequestModel>())).Returns(domainModel);

        // Act
        var response = await _planetController.Update(domainModel.Id, requestModel);

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
        var requestModel = Fixture.Create<PlanetRequestModel>();
        _planetController.ModelState.AddModelError("Bad request error", "Model is invalid");

        // Act
        var response = await _planetController.Update(Id, requestModel);

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
    public async Task Update_NonExistPlanet_ThrowsNotFoundException()
    {
        // Arrange
        var requestModel = Fixture.Create<PlanetRequestModel>();
        _mockPlanetBusinessLogic.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PlanetDomainModel>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () =>  _planetController.Update(Id, requestModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<NotFoundException>(action);
        }
    }
    #endregion
}

internal static class PlanetControllerTestsExtensions
{
    public static PlanetResponseModel ConvertToResponsePlanet(PlanetDomainModel domainModel)
    {
        return new PlanetResponseModel
        {
            Id = domainModel.Id,
            Name = domainModel.Name,
            Age = domainModel.Age,
            Air = domainModel.Air,
            Location = domainModel.Location
        };
    }

    public static List<PlanetResponseModel> ConvertToResponsePlanets(List<PlanetDomainModel> planets, int count)
    {
        var responseModels = new List<PlanetResponseModel>(count);
        planets.ForEach(planet =>
        {
            var responseModel = new PlanetResponseModel
            {
                Id = planet.Id,
                Name = planet.Name,
                Age = planet.Age,
                Air = planet.Air,
                Location = planet.Location
            };

            responseModels.Add(responseModel);
        });

        return responseModels;
    }

    public static PlanetDomainModel ConvertToDomainModel(PlanetRequestModel requestModel)
    {
        return new PlanetDomainModel
        {
            Id = Guid.NewGuid(),
            Name = requestModel.Name,
            Age = requestModel.Age,
            Air = requestModel.Air,
            Location = requestModel.Location
        };
    }
}
