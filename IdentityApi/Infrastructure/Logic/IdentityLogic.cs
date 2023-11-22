using IdentityApi.Domain;
using IdentityApi.Infrastructure.Exceptions;
using IdentityApi.Repository.Entities;
using IdentityApi.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityApi.Infrastructure.Logic;

public class IdentityLogic : IIdentityLogic
{
    private readonly IIdentityRepository _identityRepository;
    private readonly IConfiguration _configuration;

    public IdentityLogic(IIdentityRepository identityRepository, IConfiguration configuration)
    {
        _identityRepository = identityRepository;
        _configuration = configuration;
    }

    public async Task<TokenDomainModel> LoginAsync(LoginDomainModel domainModel)
    {
        var user = await _identityRepository.FindByNameAsync(domainModel.Username);
        if (!await _identityRepository.CheckPasswordAsync(user, domainModel.Password))
        {
            Log.Error($"The password for user: {user.UserName} is incorrect.");
            throw new UnauthorizedException($"The password for user: {user.UserName} is incorrect.");
        }

        var claims = await CreateClaims(user);

        return CreateToken(claims);
    }

    private TokenDomainModel CreateToken(List<Claim> claims)
    {
        var secret = Environment.GetEnvironmentVariable("ApiSecret")!;
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var expirationTimeStamp = DateTime.Now.AddMinutes(15);

        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["Jwt:ValidIssuer"],
            claims: claims,
            expires: expirationTimeStamp,
            signingCredentials: signingCredentials
        );

        return new TokenDomainModel
        {
            TokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions),
            Expiration = tokenOptions.ValidTo
        };
    }

    private async Task<List<Claim>> CreateClaims(ApplicationUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var userRoles = await _identityRepository.GetRolesAsync(user);
        userRoles.ForEach(role =>
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        });

        return claims;
    }


}
