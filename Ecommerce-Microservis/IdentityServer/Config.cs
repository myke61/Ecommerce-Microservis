using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Security.Claims;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource{
                Name = "roles",
                UserClaims = ["role"]
            },
        };

    public static IEnumerable<ApiResource> ApiResources =>
    new ApiResource[]
    {
        new ApiResource("productApiResource", "Product API Resource")
        {
            Scopes = { "productApi" },
            UserClaims = { "role" } // role claim'ini access token'a yansýtmak için bu þart
        },
        new ApiResource("basketApiResource", "Basket API Resource")
        {
            Scopes = { "basketApi" },
            UserClaims = { "role" } // role claim'ini access token'a yansýtmak için bu þart
        },
        new ApiResource("orderApiResource", "Order API Resource")
        {
            Scopes = { "orderApi" },
            UserClaims = { "role" } // role claim'ini access token'a yansýtmak için bu þart
        }
    };
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("productApi","Product API",["role"]),
            new ApiScope("basketApi","Basket API",["role"]),
            new ApiScope("orderApi","Order API",["role"]),
        };


    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "productSwagger",
                ClientName = "Swagger",
                ClientSecrets = {new Secret("secret".Sha256())},

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = {"https://localhost:7056/swagger/oauth2-redirect.html"},
                AllowedCorsOrigins = {"https://localhost:7056"},
                PostLogoutRedirectUris = {"https://localhost:7056/swagger/oauth2-redirect.html"},
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Email,
                    "productApi",
                    "basketApi",
                    "roles"
                },
                AllowAccessTokensViaBrowser =true,
                EnableLocalLogin = true,    
            },
            new Client
            {
                ClientId = "ApiDog",
                ClientName = "Api",
                ClientSecrets = {new Secret("secret".Sha256())},

                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                RedirectUris = {"https://localhost:7056/swagger/oauth2-redirect.html"},
                AllowedCorsOrigins = {"https://localhost:7056"},
                PostLogoutRedirectUris = {"https://localhost:7056/swagger/oauth2-redirect.html"},
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Email,
                    "productApi",
                    "basketApi",
                    "orderApi",
                    "roles"
                },
                AllowAccessTokensViaBrowser =true,
                EnableLocalLogin = true,
            }
        };
}
