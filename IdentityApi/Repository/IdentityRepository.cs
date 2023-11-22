using IdentityApi.Repository.Entities;
using IdentityApi.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace IdentityApi.Repository;

public class IdentityRepository : IIdentityRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApplicationUser> FindByNameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            Log.Information($"The user with username {username} doesn`t exist in the database.");
        }

        return user;
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<List<string>> GetRolesAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        return roles.ToList();
    }

    public async Task CreateAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception($"The user: {user.UserName} wasn`t created. {result}");
        }
    }
}
