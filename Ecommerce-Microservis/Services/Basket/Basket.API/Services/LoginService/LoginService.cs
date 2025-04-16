using System.Security.Claims;

namespace Basket.API.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public LoginService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId => new(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
    }
}
