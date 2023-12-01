using MassTransit;
using PlanetApi.Domain;
using PlanetApi.Infrastructure.Exceptions;
using PlanetApi.Infrastructure.Logger;
using PlanetApi.Infrastructure.Logic;
using PlanetApi.Repository.Interfaces;

namespace PlanetApi.Test.BusinessLogic;

public class PlanetBusinessLogicTests
{
    private readonly IPlanetBusinessLogic _planetBusinessLogic;
    private readonly Mock<IPlanetRepository> _mockPlanetRepository;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILoggerService> _mockLoggerService;
    private static readonly Fixture Fixture = new();
    private readonly Guid Id = Guid.NewGuid();
    private const int _planetCount = 5;

    public PlanetBusinessLogicTests()
    {
        _mockPlanetRepository = new Mock<IPlanetRepository>();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLoggerService = new Mock<ILoggerService>();
        _planetBusinessLogic = new PlanetBusinessLogic(_mockPlanetRepository.Object, _mockPublishEndpoint.Object, _mockLoggerService.Object);
    }

    private static List<PlanetDomainModel> TestPlanets()
    {
        return Fixture.CreateMany<PlanetDomainModel>(_planetCount).ToList();
    }

    #region GetAll
    [Fact]
    public async Task GetAllPlanetsAsync_ThereArePlanets_ReturnsPlanets()
    {
        // Arrange
        var tempPlanets = TestPlanets();
        _mockPlanetRepository.Setup(_ => _.GetAllAsync()).ReturnsAsync(tempPlanets);

        // Act
        var planets = await _planetBusinessLogic.GetAllPlanetsAsync();

        // Assert
        using (var scope = new AssertionScope())
        {
            planets.Count.Should().Be(tempPlanets.Count);
            planets.Should().BeEquivalentTo(tempPlanets);
        }
    }

    [Fact]
    public async Task GetAllPlanetsAsync_ThereAreNoPlanets_ReturnsEmptyList()
    {
        // Arrange
        var tempPlanets = new List<PlanetDomainModel>();
        _mockPlanetRepository.Setup(_ => _.GetAllAsync()).ReturnsAsync(tempPlanets);

        // Act
        var planets = await _planetBusinessLogic.GetAllPlanetsAsync();

        // Assert
        using (var scope = new AssertionScope())
        {
            planets.Count.Should().Be(tempPlanets.Count);
            planets.Should().BeEquivalentTo(tempPlanets);
        }
    }
    #endregion

    #region GetById
    [Fact]
    public async Task GetByIdAsync_ThereIsPlanet_ReturnsPlanet()
    {
        // Arrange
        var domainModel = Fixture.Create<PlanetDomainModel>();
        _mockPlanetRepository.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(domainModel);

        // Act
        var planet = await _planetBusinessLogic.GetByIdAsync(domainModel.Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            planet.Should().NotBeNull();

            planet.Should().BeEquivalentTo(domainModel);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ThereIsNoPlanet_ThrowsNotFoundException()
    {
        // Arrange
        _mockPlanetRepository.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _planetBusinessLogic.GetByIdAsync(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<NotFoundException>(action);
        }
    }

    [Fact]
    public async Task GetByIdAsync_IdIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        _mockPlanetRepository.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new ArgumentException());

        // Act
        var action = () => _planetBusinessLogic.GetByIdAsync(Guid.Empty);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        }
    }
    #endregion

    #region Delete
    [Fact]
    public async Task DeleteAsync_ThereIsPlanet_ReturnsCompletedTask()
    {
        // Arrange
        _mockPlanetRepository.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

        // Act
        var removing = _planetBusinessLogic.DeleteAsync(Id);
        await removing;

        // Assert
        using (var scope = new AssertionScope())
        {
            removing.Should().BeEquivalentTo(Task.CompletedTask);
        }
    }

    [Fact]
    public async Task DeleteAsync_ThereIsNoPlanet_ThrowsNotFoundException()
    {
        // Arrange
        _mockPlanetRepository.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _planetBusinessLogic.DeleteAsync(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<NotFoundException>(action);
        }
    }

    [Fact]
    public async Task DeleteAsync_IdIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        _mockPlanetRepository.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new ArgumentException());

        // Act
        var action = () => _planetBusinessLogic.DeleteAsync(Guid.Empty);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        }
    }
    #endregion

    #region Update
    [Fact]
    public async Task UpdateAsync_ValidPlanet_ReturnsPlanet()
    {
        // Arrange
        var domainModel = Fixture.Create<PlanetDomainModel>();
        _mockPlanetRepository.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PlanetDomainModel>())).ReturnsAsync(domainModel);

        // Act
        domainModel.Name = "Changed";
        var planet = await _planetBusinessLogic.UpdateAsync(Id, domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            planet.Should().NotBeNull();

            planet.Should().BeEquivalentTo(domainModel);
        }
    }

    [Fact]
    public async Task UpdateAsync_ModelIsNull_ReturnsPlanet()
    {
        // Arrange
        PlanetDomainModel domainModel = null;

        // Act
        var action = () => _planetBusinessLogic.UpdateAsync(Id, domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
        }
    }

    [Fact]
    public async Task UpdateAsync_ThereIsNoPlanet_ThrowsNotFoundException()
    {
        // Arrange
        var domainModel = Fixture.Create<PlanetDomainModel>();
        _mockPlanetRepository.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PlanetDomainModel>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _planetBusinessLogic.UpdateAsync(Id, domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<NotFoundException>(action);
        }
    }

    [Fact]
    public async Task UpdateAsync_IdIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var domainModel = Fixture.Create<PlanetDomainModel>();
        _mockPlanetRepository.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PlanetDomainModel>())).ThrowsAsync(new ArgumentException());

        // Act
        var action = () => _planetBusinessLogic.UpdateAsync(Guid.Empty, domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        }
    }
    #endregion

    #region Create
    [Fact]
    public async Task CreateAsync_ValidPlanet_ReturnsPlanet()
    {
        // Arrange
        var domainModel = Fixture.Create<PlanetDomainModel>();
        _mockPlanetRepository.Setup(_ => _.CreateAsync(It.IsAny<PlanetDomainModel>())).ReturnsAsync(domainModel);

        // Act
        var planet = await _planetBusinessLogic.CreateAsync(domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            planet.Should().NotBeNull();

            planet.Should().BeEquivalentTo(domainModel);
        }
    }

    [Fact]
    public async Task CreateAsync_CountryIsNotCreated_ThrowsArgumentNullException()
    {
        // Arrange
        var domainModel = Fixture.Create<PlanetDomainModel>();
        _mockPlanetRepository.Setup(_ => _.CreateAsync(It.IsAny<PlanetDomainModel>())).ThrowsAsync(new Exception());

        // Act
        var action = () => _planetBusinessLogic.CreateAsync(domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<Exception>(action);
        }
    }

    [Fact]
    public async Task CreateAsync_ModelIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        PlanetDomainModel domainModel = null;

        // Act
        var action = () => _planetBusinessLogic.CreateAsync(domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
        }
    }
    #endregion
}
