using AutoMapper;
using IdentityApi.Boundary;
using IdentityApi.Domain;
using IdentityApi.Infrastructure.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IdentityApi.Controllers;

[ApiController]
[Route("api/v1/identity")]
[ApiExplorerSettings(GroupName = "v1")]
public class IdentityController : BaseController
{
    private readonly IIdentityLogic _identityLogic;
    private readonly IMapper _mapper;

    public IdentityController(IIdentityLogic identityLogic, IMapper mapper)
    {
        _identityLogic = identityLogic;
        _mapper = mapper;
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new BaseResponseModel(GetErrorMessage(ModelState), HttpStatusCode.BadRequest));
        }

        var token = await _identityLogic.LoginAsync(_mapper.Map<LoginDomainModel>(model));

        return Ok(token);
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new BaseResponseModel(GetErrorMessage(ModelState), HttpStatusCode.BadRequest));
        }

        await _identityLogic.RegisterAsync(_mapper.Map<RegisterDomainModel>(model));

        return Ok("User successully created");
    }
}
