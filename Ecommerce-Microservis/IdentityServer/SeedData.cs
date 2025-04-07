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
    public static async void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            RoleManager<IdentityRole> roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var adminRole = roleMgr.FindByNameAsync("admin").Result;
            var userRole = roleMgr.FindByNameAsync("user").Result;
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
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(mikail, [
                            new(JwtClaimTypes.Name, "Mikail A��rman"),
                            new(JwtClaimTypes.GivenName, "Mikail"),
                            new(JwtClaimTypes.FamilyName, "A��rman"),
                            new(JwtClaimTypes.Address, "Turkey"),
                        ]).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                result = userMgr.AddToRoleAsync(mikail, "admin").Result;
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
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, [
                            new(JwtClaimTypes.Name, "Bob Smith"),
                            new(JwtClaimTypes.GivenName, "Bob"),
                            new(JwtClaimTypes.FamilyName, "Smith"),
                            new("location", "Russia")
                        ]).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                result = userMgr.AddToRoleAsync(bob, "user").Result;
                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
        }
    }
}
