using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Boundary.Human;
using MinimalApi.Boundary.Human.RequestModel;
using MinimalApi.Controllers;
using MinimalApi.Domain;
using MinimalApi.Infrastructure.Exceptions;
using MinimalApi.Infrastructure.Logic.Interfaces;
using System.Net;

namespace PopulationApi.Test.Controllers;

public class HumanControllerTests
{
    private readonly HumanController _humanController;
    private readonly Mock<IHumanBusinessLogic> _mockHumanBusinessLogic;
    private readonly Mock<IMapper> _mockMapper;
    private static readonly Fixture Fixture = new();
    private readonly Guid Id = Guid.NewGuid();
    private const int _humanCount = 5;

    public HumanControllerTests()
    {
        _mockHumanBusinessLogic = new Mock<IHumanBusinessLogic>();
        _mockMapper = new Mock<IMapper>();
        _humanController = new HumanController(_mockHumanBusinessLogic.Object, _mockMapper.Object);
    }

    private static List<HumanDomainModel> TestPeople()
    {
        return Fixture.CreateMany<HumanDomainModel>(_humanCount).ToList();
    }

    #region GetAll
    [Fact]
    public async Task GetAll_ThereArePeople_ReturnsPeople()
    {
        // Arrange
        var tempPeople = TestPeople();
        var responseModels = HumanControllerTestsExtensions.ConvertToResponsePeople(tempPeople, tempPeople.Count);
        _mockHumanBusinessLogic.Setup(_ => _.GetAllPeopleAsync()).ReturnsAsync(tempPeople);
        _mockMapper.Setup(_ => _.Map<List<HumanResponseModel>>(It.IsAny<List<HumanDomainModel>>())).Returns(responseModels);

        // Act
        var response = await _humanController.GetAll();

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult!.Value.Should().NotBeNull();

            var responseValue = okObjectResult.Value;
            var people = responseValue as List<HumanResponseModel>;

            people.Should().NotBeNull();
            people!.Count.Should().Be(_humanCount);
            people.Should().BeEquivalentTo(responseModels);
        }
    }

    [Fact]
    public async Task GetAll_ThereAreNoPeople_ReturnsEmptyList()
    {
        // Arrange
        var tempPeople = new List<HumanDomainModel>();
        var responseModels = HumanControllerTestsExtensions.ConvertToResponsePeople(tempPeople, tempPeople.Count);
        _mockHumanBusinessLogic.Setup(_ => _.GetAllPeopleAsync()).ReturnsAsync(tempPeople);
        _mockMapper.Setup(_ => _.Map<List<HumanResponseModel>>(It.IsAny<List<HumanDomainModel>>())).Returns(responseModels);

        // Act
        var response = await _humanController.GetAll();

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult!.Value.Should().NotBeNull();

            var responseValue = okObjectResult.Value;
            var people = responseValue as List<HumanResponseModel>;

            people.Should().NotBeNull();
            people!.Count.Should().Be(0);
            people.Should().BeEquivalentTo(responseModels);
        }
    }
    #endregion GetAll

    #region GetById
    [Fact]
    public async Task GetById_ThereIsHuman_ReturnsHuman()
    {
        // Arrange
        var domainModel = Fixture.Create<HumanDomainModel>();
        var responseModel = HumanControllerTestsExtensions.ConvertToResponseHuman(domainModel);
        _mockHumanBusinessLogic.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(domainModel);
        _mockMapper.Setup(_ => _.Map<HumanResponseModel>(It.IsAny<HumanDomainModel>())).Returns(responseModel);

        // Act
        var response = await _humanController.GetById(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult!.Value.Should().NotBeNull();

            var responseValue = okObjectResult.Value;
            var county = responseValue as HumanResponseModel;

            county.Should().NotBeNull();
            county.Should().BeEquivalentTo(responseModel);
        }
    }

    [Fact]
    public async Task GetById_ThereIsNoHuman_ReturnsNotFound()
    {
        // Arrange
        _mockHumanBusinessLogic.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var response = await _humanController.GetById(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var notFoundObjectResult = response as NotFoundObjectResult;
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            notFoundObjectResult!.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeEquivalentTo(new Tuple<string, HttpStatusCode>($"Human with id: {Id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }
    #endregion GetById

    #region Delete
    [Fact]
    public async Task Delete_ThereIsHuman_ReturnsNoContent()
    {
        // Arrange
        _mockHumanBusinessLogic.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

        // Act
        var response = await _humanController.Delete(Id);

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
    public async Task Delete_ThereIsNoHuman_ReturnsNotFound()
    {
        // Arrange
        _mockHumanBusinessLogic.Setup(_ => _.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException());

        // Act
        var response = await _humanController.Delete(Id);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var notFoundObjectResult = response as NotFoundObjectResult;
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            notFoundObjectResult!.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeEquivalentTo(new Tuple<string, HttpStatusCode>($"Human with id: {Id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }
    #endregion

    #region Create
    [Fact]
    public async Task Create_ValidModel_ReturnsHuman()
    {
        // Arrange
        var requestModel = Fixture.Create<HumanCreateRequestModel>();
        var domainModel = HumanControllerTestsExtensions.ConvertToDomainModel(requestModel);
        var responseModel = HumanControllerTestsExtensions.ConvertToResponseHuman(domainModel);
        _mockHumanBusinessLogic.Setup(_ => _.CreateAsync(It.IsAny<HumanDomainModel>())).ReturnsAsync(domainModel);
        _mockMapper.Setup(_ => _.Map<HumanResponseModel>(It.IsAny<HumanDomainModel>())).Returns(responseModel);
        _mockMapper.Setup(_ => _.Map<HumanDomainModel>(It.IsAny<HumanCreateRequestModel>())).Returns(domainModel);

        // Act
        var response = await _humanController.Create(requestModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var createdAtActionResult = response as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            createdAtActionResult!.Value.Should().NotBeNull();

            var responseValue = createdAtActionResult.Value;
            var county = responseValue as HumanResponseModel;

            county.Should().NotBeNull();
            county.Should().BeEquivalentTo(responseModel);
        }
    }

    [Fact]
    public async Task Create_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var requestModel = Fixture.Create<HumanCreateRequestModel>();
        _humanController.ModelState.AddModelError("Bad request error", "Model is invalid");

        // Act
        var response = await _humanController.Create(requestModel);

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
        var requestModel = Fixture.Create<HumanUpdateRequestModel>();
        var domainModel = HumanControllerTestsExtensions.ConvertToDomainModel(requestModel);
        var responseModel = HumanControllerTestsExtensions.ConvertToResponseHuman(domainModel);
        _mockHumanBusinessLogic.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<HumanDomainModel>())).ReturnsAsync(domainModel);
        _mockMapper.Setup(_ => _.Map<HumanResponseModel>(It.IsAny<HumanDomainModel>())).Returns(responseModel);
        _mockMapper.Setup(_ => _.Map<HumanDomainModel>(It.IsAny<HumanUpdateRequestModel>())).Returns(domainModel);

        // Act
        var response = await _humanController.Update(domainModel.Id, requestModel);

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
        var requestModel = Fixture.Create<HumanUpdateRequestModel>();
        _humanController.ModelState.AddModelError("Bad request error", "Model is invalid");

        // Act
        var response = await _humanController.Update(Id, requestModel);

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
    public async Task Update_NonExistHuman_ReturnsNotFound()
    {
        // Arrange
        var requestModel = Fixture.Create<HumanUpdateRequestModel>();
        _mockHumanBusinessLogic.Setup(_ => _.UpdateAsync(It.IsAny<Guid>(), It.IsAny<HumanDomainModel>())).ThrowsAsync(new NotFoundException());

        // Act
        var response = await _humanController.Update(Id, requestModel);

        // Assert
        using (var scope = new AssertionScope())
        {
            response.Should().NotBeNull();

            var notFoundObjectResult = response as NotFoundObjectResult;
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            notFoundObjectResult!.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeEquivalentTo(new Tuple<string, HttpStatusCode>($"Human with id: {Id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }
    #endregion
}

internal static class HumanControllerTestsExtensions
{
    public static HumanResponseModel ConvertToResponseHuman(HumanDomainModel domainModel)
    {
        return new HumanResponseModel
        {
            Id = domainModel.Id,
            Name = domainModel.Name,
            Age = domainModel.Age,
            Gender = domainModel.Gender,
            Surname = domainModel.Surname,
            CountryId = (Guid)domainModel.CountryId!
        };
    }

    public static List<HumanResponseModel> ConvertToResponsePeople(List<HumanDomainModel> people, int count)
    {
        var responseModels = new List<HumanResponseModel>(count);
        people.ForEach(human =>
        {
            var responseModel = new HumanResponseModel
            {
                Id = human.Id,
                Name = human.Name,
                Age = human.Age,
                Gender = human.Gender,
                Surname = human.Surname,
                CountryId = (Guid)human.CountryId!
            };

            responseModels.Add(responseModel);
        });

        return responseModels;
    }

    public static HumanDomainModel ConvertToDomainModel(HumanCreateRequestModel requestModel)
    {
        return new HumanDomainModel
        {
            Id = Guid.NewGuid(),
            Name = requestModel.Name,
            Age = requestModel.Age,
            Gender = requestModel.Gender,
            Surname = requestModel.Surname,
            CountryId = (Guid)requestModel.CountryId
        };
    }

    public static HumanDomainModel ConvertToDomainModel(HumanUpdateRequestModel requestModel)
    {
        return new HumanDomainModel
        {
            Id = Guid.NewGuid(),
            Name = requestModel.Name,
            Age = requestModel.Age,
            Gender = requestModel.Gender,
            Surname = requestModel.Surname,
            CountryId = (Guid)requestModel.CountryId
        };
    }
}
