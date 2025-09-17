using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LoginService.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public LoginService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId => new(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());

        public string UserName => httpContextAccessor.HttpContext?.User.FindFirst("name")?.Value;
    }
}
