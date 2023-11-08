using MinimalApi.Domain;
using MinimalApi.Infrastructure.Exceptions;
using MinimalApi.Infrastructure.Logic;
using MinimalApi.Infrastructure.Logic.Interfaces;
using MinimalApi.Repository.Interfaces;

namespace PopulationApi.Test.BusinessLogic;

public class CountryBusinessLogicTests
{
    private readonly ICountryBusinessLogic _countryBusinessLogic;
    private readonly Mock<ICountryRepository> _mockCountryRepository;
    private static readonly Fixture Fixture = new();
    private readonly Guid Id = Guid.NewGuid();
    private const int _countryCount = 5;

    public CountryBusinessLogicTests()
    {
        _mockCountryRepository = new Mock<ICountryRepository>();
        _countryBusinessLogic = new CountryBusinessLogic(_mockCountryRepository.Object);
    }

    private static List<CountryDomainModel> TestCountries()
    {
        return Fixture.CreateMany<CountryDomainModel>(_countryCount).ToList();
    }

    #region GetAll
    [Fact]
    public async Task GetAllCountriesAsync_ThereAreCountries_ReturnsCountries()
    {
        // Arrange
        var tempCountries = TestCountries();
        _mockCountryRepository.Setup(_ => _.GetAllAsync()).ReturnsAsync(tempCountries);

        // Act
        var countries = await _countryBusinessLogic.GetAllCountriesAsync();

        // Assert
        using (var scope = new AssertionScope())
        {
            countries.Count.Should().Be(tempCountries.Count);
            countries.Should().BeEquivalentTo(tempCountries);
        }
    }
    
    [Fact]
    public async Task GetAllCountriesAsync_ThereAreNoCountries_ReturnsEmptyList()
    {
        // Arrange
        var tempCountries = new List<CountryDomainModel>();
        _mockCountryRepository.Setup(_ => _.GetAllAsync()).ReturnsAsync(tempCountries);

        // Act
        var countries = await _countryBusinessLogic.GetAllCountriesAsync();

        // Assert
        using (var scope = new AssertionScope())
        {
            countries.Count.Should().Be(tempCountries.Count);
            countries.Should().BeEquivalentTo(tempCountries);
        }
    }
    #endregion

    #region GetById
    [Fact]
    public async Task GetByIdAsync_ThereIsCountry_ReturnsCountry()
    {
        // Arrange
        var domainModel = Fixture.Create<CountryDomainModel>();
        _mockCountryRepository.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(domainModel);

        // Act
        var country = await _countryBusinessLogic.GetByIdAsync(domainModel.Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            country.Should().NotBeNull();

            country.Should().BeEquivalentTo(domainModel);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ThereIsNoCountry_ThrowsNotFoundException()
    {
        // Arrange
        _mockCountryRepository.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _countryBusinessLogic.GetByIdAsync(Id);

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
        _mockCountryRepository.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new ArgumentException());

        // Act
        var action = () => _countryBusinessLogic.GetByIdAsync(Guid.Empty);

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
    public async Task DeleteAsync_ThereIsCountry_ReturnsCompletedTask()
    {
        // Arrange
        _mockCountryRepository.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

        // Act
        var removing = _countryBusinessLogic.DeleteAsync(Id);
        await removing;

        // Assert
        using (var scope = new AssertionScope())
        {
            removing.Should().BeEquivalentTo(Task.CompletedTask);
        }
    }

    [Fact]
    public async Task DeleteAsync_ThereIsNoCountry_ThrowsNotFoundException()
    {
        // Arrange
        _mockCountryRepository.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _countryBusinessLogic.DeleteAsync(Id);

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
        _mockCountryRepository.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new ArgumentException());

        // Act
        var action = () => _countryBusinessLogic.DeleteAsync(Guid.Empty);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        }
    }
    #endregion

    #region Update
    [Fact]
    public async Task UpdateAsync_ValidCountry_ReturnsCountry()
    {
        // Arrange
        var domainModel = Fixture.Create<CountryDomainModel>();
        _mockCountryRepository.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CountryDomainModel>())).ReturnsAsync(domainModel);

        // Act
        domainModel.Name = "Changed";
        var country = await _countryBusinessLogic.UpdateAsync(Id, domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            country.Should().NotBeNull();

            country.Should().BeEquivalentTo(domainModel);
        }
    }

    [Fact]
    public async Task UpdateAsync_ModelIsNull_ReturnsCountry()
    {
        // Arrange
        CountryDomainModel domainModel = null;

        // Act
        var action = () => _countryBusinessLogic.UpdateAsync(Id, domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
        }
    }

    [Fact]
    public async Task UpdateAsync_ThereIsNoCountry_ThrowsNotFoundException()
    {
        // Arrange
        var domainModel = Fixture.Create<CountryDomainModel>();
        _mockCountryRepository.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CountryDomainModel>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _countryBusinessLogic.UpdateAsync(Id, domainModel);

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
        var domainModel = Fixture.Create<CountryDomainModel>();
        _mockCountryRepository.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CountryDomainModel>())).ThrowsAsync(new ArgumentException());

        // Act
        var action = () => _countryBusinessLogic.UpdateAsync(Guid.Empty, domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        }
    }
    #endregion

    #region Create
    [Fact]
    public async Task CreateAsync_ValidCountry_ReturnsCountry()
    {
        // Arrange
        var domainModel = Fixture.Create<CountryDomainModel>();
        _mockCountryRepository.Setup(_ => _.CreateAsync(It.IsAny<CountryDomainModel>())).ReturnsAsync(domainModel);

        // Act
        domainModel.Name = "Changed";
        var country = await _countryBusinessLogic.CreateAsync(domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            country.Should().NotBeNull();

            country.Should().BeEquivalentTo(domainModel);
        }
    }

    [Fact]
    public async Task CreateAsync_ThereIsNoCountry_ThrowsNotFoundException()
    {
        // Arrange
        var domainModel = Fixture.Create<CountryDomainModel>();
        _mockCountryRepository.Setup(_ => _.CreateAsync(It.IsAny<CountryDomainModel>())).ThrowsAsync(new NotFoundException());

        // Act
        var action = () => _countryBusinessLogic.CreateAsync(domainModel);

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
        CountryDomainModel domainModel = null;

        // Act
        var action = () => _countryBusinessLogic.CreateAsync(domainModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
        }
    }
    #endregion
}
