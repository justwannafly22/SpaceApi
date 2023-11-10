using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PopulationApi.Boundary;
using PopulationApi.Boundary.Human;
using PopulationApi.Boundary.Human.RequestModel;
using PopulationApi.Domain;
using PopulationApi.Infrastructure.Exceptions;
using PopulationApi.Infrastructure.Logic.Interfaces;
using System.Net;

namespace PopulationApi.Controllers;

[ApiController]
[Route("api/v1/people")]
[ApiExplorerSettings(GroupName = "v1")]
public class HumanController : BaseController
{
    private readonly IHumanBusinessLogic _humanBusinessLogic;
    private readonly IMapper _mapper;

    public HumanController(IHumanBusinessLogic humanBusinessLogic, IMapper mapper)
    {
        _humanBusinessLogic = humanBusinessLogic;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns all people
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = _mapper.Map<List<HumanResponseModel>>(await _humanBusinessLogic.GetAllPeopleAsync().ConfigureAwait(false));

        return Ok(response);
    }

    /// <summary>
    /// Returns a human
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var human = await _humanBusinessLogic.GetByIdAsync(id).ConfigureAwait(false);

            return Ok(_mapper.Map<HumanResponseModel>(human));
        }
        catch (NotFoundException)
        {
            return NotFound(($"Human with id: {id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }

    /// <summary>
    /// Create a human
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HumanCreateRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new BaseResponseModel(GetErrorMessage(ModelState), HttpStatusCode.BadRequest));
        }

        var addedHuman = await _humanBusinessLogic.CreateAsync(_mapper.Map<HumanDomainModel>(model)).ConfigureAwait(false);

        return CreatedAtAction(nameof(Create), _mapper.Map<HumanResponseModel>(addedHuman));
    }

    /// <summary>
    /// Update a human
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] HumanUpdateRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponseModel(GetErrorMessage(ModelState), HttpStatusCode.BadRequest));
            }

            _ = await _humanBusinessLogic.UpdateAsync(id, _mapper.Map<HumanDomainModel>(model)).ConfigureAwait(false);

            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound(($"Human with id: {id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }

    /// <summary>
    /// Delete a human
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _humanBusinessLogic.DeleteAsync(id).ConfigureAwait(false);

            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound(($"Human with id: {id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }
}
