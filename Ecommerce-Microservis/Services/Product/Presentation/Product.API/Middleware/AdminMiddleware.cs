using Duende.IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Product.API.Middleware
{
    public class AdminMiddleware
    {
        private readonly RequestDelegate _next;
        public AdminMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (path != null && path.StartsWith("/api/productAdmin", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("https://localhost:5001");
                    var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    client.SetBearerToken(token);

                    var response = await client.GetAsync("/connect/userinfo");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var userInfo = JObject.Parse(content);
                        var role = userInfo["role"].ToString();

                        if(role != "Admin")
                        {
                            throw new UnauthorizedAccessException("You Shall Not Pass!!");
                        }
                    }
                    else
                    {
                        throw new Exception("Kullanıcı bilgileri alınamadı.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            await _next(context);
        }
    }
}
