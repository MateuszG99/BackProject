using Application.Interfaces;
using Domain.BusinessModels;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.Identity
{
    public class IdentityService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : IIdentityService
    {
        public bool RegisterNewAdmin(AppUser input)
        {
            IdentityUser user = new()
            {
                UserName = input.Login,
                Email = $"{input.Login}@domain.com"
            };
            var creationResult = userManager.CreateAsync(user, input.Password).Result;

            if (!creationResult.Succeeded)
                return false;

            if (!roleManager.RoleExistsAsync(Domain.Constants.Administrator).Result)
            {
                var role = new IdentityRole
                {
                    Name = Domain.Constants.Administrator
                };
                var roleCreationResult = roleManager.CreateAsync(role).Result;

                if (!roleCreationResult.Succeeded)
                    return false;
            }

            var roleAssignmentResult = userManager.AddToRoleAsync(user, Domain.Constants.Administrator).Result;

            if (!roleAssignmentResult.Succeeded)
                return false;

            return true;
        }

        public bool RegisterNewModerator(AppUser input)
        {
            IdentityUser user = new()
            {
                UserName = input.Login,
                Email = $"{input.Login}@domain.com"
            };
            var creationResult = userManager.CreateAsync(user, input.Password).Result;

            if (!creationResult.Succeeded)
                return false;

            if (!roleManager.RoleExistsAsync(Domain.Constants.Moderator).Result)
            {
                var role = new IdentityRole
                {
                    Name = Domain.Constants.Moderator
                };
                var roleCreationResult = roleManager.CreateAsync(role).Result;

                if (!roleCreationResult.Succeeded)
                    return false;
            }

            var roleAssignmentResult = userManager.AddToRoleAsync(user, Domain.Constants.Moderator).Result;

            if (!roleAssignmentResult.Succeeded)
                return false;

            return true;
        }

        public string Login(AppUser input)
        {
            var user = userManager.FindByNameAsync(input.Login).Result;

            if (user is null)
                return "User Not Found!";

            var loginResult = userManager.CheckPasswordAsync(user, input.Password).Result;

            if (!loginResult)
                return "Invalid Password!";

            var calimsList = new List<Claim>();
            foreach (var role in userManager.GetRolesAsync(user).Result)
                calimsList.Add(new Claim(ClaimTypes.Role, role));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenOptions:Key"]!));

            var token = new JwtSecurityToken(
                issuer: configuration["TokenOptions:ValidIssuer"],
                audience: configuration["TokenOptions:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: calimsList,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

