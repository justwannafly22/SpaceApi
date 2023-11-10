using PopulationApi.Domain;
using PopulationApi.Infrastructure.Exceptions;
using PopulationApi.Infrastructure.Logic;
using PopulationApi.Infrastructure.Logic.Interfaces;
using PopulationApi.Repository.Interfaces;

namespace PopulationApi.Test.BusinessLogic;

public class HumanBusinessLogicTests
{
    private readonly IHumanBusinessLogic _humanBusinessLogic;
    private readonly Mock<IHumanRepository> _mockHumanRepository;
    private static readonly Fixture Fixture = new();
    private readonly Guid Id = Guid.NewGuid();
    private const int _humanCount = 5;

    public HumanBusinessLogicTests()
    {
        _mockHumanRepository = new Mock<IHumanRepository>();
        _humanBusinessLogic = new HumanBusinessLogic(_mockHumanRepository.Object);
    }

    private static List<HumanDomainModel> TestPeople()
    {
        return Fixture.CreateMany<HumanDomainModel>(_humanCount).ToList();
    }

    #region GetAll
    [Fact]
    public async Task GetAllPeopleAsync_ThereArePeople_ReturnsPeople()
    {
        // Arrange
        var tempPeople = TestPeople();
        _mockHumanRepository.Setup(_ => _.GetAllAsync()).ReturnsAsync(tempPeople);

        // Act
        var countries = await _humanBusinessLogic.GetAllPeopleAsync();

        // Assert
        using (var scope = new AssertionScope())
        {
            countries.Count.Should().Be(tempPeople.Count);
            countries.Should().BeEquivalentTo(tempPeople);
        }
    }

    [Fact]
    public async Task GetAllPeopleAsync_ThereAreNoPeople_ReturnsEmptyList()
    {
        // Arrange
        var tempPeople = new List<HumanDomainModel>();
        _mockHumanRepository.Setup(_ => _.GetAllAsync()).ReturnsAsync(tempPeople);

        // Act
        var countries = await _humanBusinessLogic.GetAllPeopleAsync();

        // Assert
        using (var scope = new AssertionScope())
        {
            countries.Count.Should().Be(tempPeople.Count);
            countries.Should().BeEquivalentTo(tempPeople);
        }
    }
    #endregion

    #region GetById
    [Fact]
    public async Task GetByIdAsync_ThereIsHuman_ReturnsHuman()
    {
        // Arrange
        var domainModel = Fixture.Create<HumanDomainModel>();
        _mockHumanRepository.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(domainModel);

        // Act
        var human = await _humanBusinessLogic.GetByIdAsync(domainModel.Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            human.Should().NotBeNull();

            human.Should().BeEquivalentTo(domainModel);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ThereIsNoHuman_ThrowsNotFoundException()
    {
        // Arrange
        _mockHumanRepository.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _humanBusinessLogic.GetByIdAsync(Id);

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
        _mockHumanRepository.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new ArgumentException());

        // Act
        var action = () => _humanBusinessLogic.GetByIdAsync(Guid.Empty);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        }
    }
    #endregion

    #region Delete
    // ToDo: Find a better way to test the removing process. Pay attention to Act and Assert sections.
    [Fact]
    public async Task DeleteAsync_ThereIsHuman_ReturnsCompletedTask()
    {
        // Arrange
        _mockHumanRepository.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

        // Act
        var removing = _humanBusinessLogic.DeleteAsync(Id);
        await removing;

        // Assert
        using (var scope = new AssertionScope())
        {
            removing.Should().BeEquivalentTo(Task.CompletedTask);
        }
    }

    [Fact]
    public async Task DeleteAsync_ThereIsNoHuman_ThrowsNotFoundException()
    {
        // Arrange
        _mockHumanRepository.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _humanBusinessLogic.DeleteAsync(Id);

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
        _mockHumanRepository.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new ArgumentException());

        // Act
        var action = () => _humanBusinessLogic.DeleteAsync(Guid.Empty);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        }
    }
    #endregion

    #region Update
    [Fact]
    public async Task UpdateAsync_ValidHuman_ReturnsHuman()
    {
        // Arrange
        var domainModel = Fixture.Create<HumanDomainModel>();
        _mockHumanRepository.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<HumanDomainModel>())).ReturnsAsync(domainModel);

        // Act
        domainModel.Name = "Changed";
        var human = await _humanBusinessLogic.UpdateAsync(Id, domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            human.Should().NotBeNull();

            human.Should().BeEquivalentTo(domainModel);
        }
    }

    [Fact]
    public async Task UpdateAsync_ModelIsNull_ReturnsHuman()
    {
        // Arrange
        HumanDomainModel domainModel = null;

        // Act
        var action = () => _humanBusinessLogic.UpdateAsync(Id, domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
        }
    }

    [Fact]
    public async Task UpdateAsync_ThereIsNoHuman_ThrowsNotFoundException()
    {
        // Arrange
        var domainModel = Fixture.Create<HumanDomainModel>();
        _mockHumanRepository.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<HumanDomainModel>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _humanBusinessLogic.UpdateAsync(Id, domainModel);

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
        var domainModel = Fixture.Create<HumanDomainModel>();
        _mockHumanRepository.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<HumanDomainModel>())).ThrowsAsync(new ArgumentException());

        // Act
        var action = () => _humanBusinessLogic.UpdateAsync(Guid.Empty, domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        }
    }
    #endregion

    #region Create
    [Fact]
    public async Task CreateAsync_ValidHuman_ReturnsHuman()
    {
        // Arrange
        var domainModel = Fixture.Create<HumanDomainModel>();
        _mockHumanRepository.Setup(_ => _.CreateAsync(It.IsAny<HumanDomainModel>())).ReturnsAsync(domainModel);

        // Act
        domainModel.Name = "Changed";
        var human = await _humanBusinessLogic.CreateAsync(domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            human.Should().NotBeNull();

            human.Should().BeEquivalentTo(domainModel);
        }
    }

    [Fact]
    public async Task CreateAsync_ThereIsNoHuman_ThrowsNotFoundException()
    {
        // Arrange
        var domainModel = Fixture.Create<HumanDomainModel>();
        _mockHumanRepository.Setup(_ => _.CreateAsync(It.IsAny<HumanDomainModel>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _humanBusinessLogic.CreateAsync(domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<NotFoundException>(action);
        }
    }

    [Fact]
    public async Task CreateAsync_ModelIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        HumanDomainModel domainModel = null;

        // Act
        var action = () => _humanBusinessLogic.CreateAsync(domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
        }
    }
    #endregion
}
