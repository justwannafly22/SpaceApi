using IdentityApi.Repository.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Repository.Interfaces;

public interface IIdentityRepository
{
    public Task<ApplicationUser> FindByNameAsync(string username);
    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    public Task<List<string>> GetRolesAsync(ApplicationUser user);
    Task CreateAsync(ApplicationUser user, string password);
}
