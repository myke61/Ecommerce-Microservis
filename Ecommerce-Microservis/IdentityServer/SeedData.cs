using System.Security.Claims;
using Duende.IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            RoleManager<IdentityRole> roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var adminRole = roleMgr.FindByNameAsync("Admin").Result;
            var userRole = roleMgr.FindByNameAsync("User").Result;
            if (adminRole == null)
            {
                roleMgr.CreateAsync(new IdentityRole("Admin")).Wait();
            }
            if (userRole == null)
            {
                roleMgr.CreateAsync(new IdentityRole("User")).Wait();
            }

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var mikail = userMgr.FindByNameAsync("Mikail").Result;
            if (mikail == null)
            {
                mikail = new ApplicationUser
                {
                    UserName = "Mikail",
                    Email = "mikailagirman61@gmail.com",
                    EmailConfirmed = true,
                };
                var result = userMgr.CreateAsync(mikail, "Pass123$").Result;
                result = userMgr.AddToRoleAsync(mikail, "Admin").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(mikail, [
                            new(JwtClaimTypes.Name, "Mikail Agirman"),
                            new(ClaimTypes.Name,"Mikail Agirman"),
                            new(JwtClaimTypes.GivenName, "Mikail"),
                            new(JwtClaimTypes.FamilyName, "Agirman"),
                            new(JwtClaimTypes.Address, "Turkey"),
                            new (JwtClaimTypes.Roles, "Admin"),
                        ]).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("Mikail Created");
            }
            else
            {
                Log.Debug("Mikail Already Exist");
            }

            var bob = userMgr.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    UserName = "bob",
                    Email = "BobSmith@example.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                result = userMgr.AddToRoleAsync(bob, "User").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, [
                            new(JwtClaimTypes.Name, "Bob Smith"),
                            new(ClaimTypes.Name,"Bob Smith"),
                            new(JwtClaimTypes.GivenName, "Bob"),
                            new(JwtClaimTypes.FamilyName, "Smith"),
                            new("location", "Russia"),
                            new(JwtClaimTypes.Roles, "User"),
                        ]).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
        }
    }
}
