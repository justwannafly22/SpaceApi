using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlanetApi.Boundary;
using PlanetApi.Domain;
using PlanetApi.Infrastructure.Logic;
using System.Net;

namespace PlanetApi.Controllers;

[ApiController]
[Route("api/v1/planets")]
[ApiExplorerSettings(GroupName = "v1")]
public class PlanetController : BaseController
{
    private readonly IPlanetBusinessLogic _planetBusinessLogic;
    private readonly IMapper _mapper;

    public PlanetController(IPlanetBusinessLogic planetBusinessLogic, IMapper mapper)
    {
        _planetBusinessLogic = planetBusinessLogic;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns all planets
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = _mapper.Map<List<PlanetResponseModel>>(await _planetBusinessLogic.GetAllPlanetsAsync().ConfigureAwait(false));

        return Ok(response);
    }

    /// <summary>
    /// Returns a planet
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var planet = await _planetBusinessLogic.GetByIdAsync(id).ConfigureAwait(false);

        return Ok(_mapper.Map<PlanetResponseModel>(planet));
    }

    /// <summary>
    /// Create a planet
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlanetRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new BaseResponseModel(GetErrorMessage(ModelState), HttpStatusCode.BadRequest));
        }

        var addedPlanet = await _planetBusinessLogic.CreateAsync(_mapper.Map<PlanetDomainModel>(model)).ConfigureAwait(false);

        return CreatedAtAction(nameof(Create), _mapper.Map<PlanetResponseModel>(addedPlanet));
    }

    /// <summary>
    /// Update a planet
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PlanetRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new BaseResponseModel(GetErrorMessage(ModelState), HttpStatusCode.BadRequest));
        }

        _ = await _planetBusinessLogic.UpdateAsync(id, _mapper.Map<PlanetDomainModel>(model)).ConfigureAwait(false);

        return NoContent();
    }

    /// <summary>
    /// Delete a planet
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _planetBusinessLogic.DeleteAsync(id).ConfigureAwait(false);

        return NoContent();
    }
}
