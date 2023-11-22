using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PopulationApi.Boundary;
using PopulationApi.Boundary.Country;
using PopulationApi.Boundary.Country.RequestModels;
using PopulationApi.Domain;
using PopulationApi.Infrastructure.Exceptions;
using PopulationApi.Infrastructure.Logic.Interfaces;
using System.Net;

namespace PopulationApi.Controllers;

[ApiController]
[Route("api/v1/countries")]
[ApiExplorerSettings(GroupName = "v1")]
public class CountryController : BaseController
{
    private readonly ICountryBusinessLogic _countryBusinessLogic;
    private readonly IMapper _mapper;

    public CountryController(ICountryBusinessLogic countryBusinessLogic, IMapper mapper)
    {
        _countryBusinessLogic = countryBusinessLogic;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns all countries
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = _mapper.Map<List<CountryResponseModel>>(await _countryBusinessLogic.GetAllCountriesAsync());

        return Ok(response);
    }

    /// <summary>
    /// Returns a country
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var country = await _countryBusinessLogic.GetByIdAsync(id);

            return Ok(_mapper.Map<CountryResponseModel>(country));
        }
        catch (NotFoundException)
        {
            return NotFound(($"Country with id: {id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }

    /// <summary>
    /// Create a country
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CountryCreateRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new BaseResponseModel(GetErrorMessage(ModelState), HttpStatusCode.BadRequest));
        }

        var addedCountry = await _countryBusinessLogic.CreateAsync(_mapper.Map<CountryDomainModel>(model));

        return CreatedAtAction(nameof(Create), _mapper.Map<CountryResponseModel>(addedCountry));
    }

    /// <summary>
    /// Update a country
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CountryUpdateRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponseModel(GetErrorMessage(ModelState), HttpStatusCode.BadRequest));
            }

            _ = await _countryBusinessLogic.UpdateAsync(id, _mapper.Map<CountryDomainModel>(model));

            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound(($"Country with id: {id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }

    /// <summary>
    /// Delete a country
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _countryBusinessLogic.DeleteAsync(id);

            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound(($"Country with id: {id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }
}
