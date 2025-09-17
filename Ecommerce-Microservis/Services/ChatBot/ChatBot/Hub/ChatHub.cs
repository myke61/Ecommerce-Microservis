using LoginService.LoginService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Hub
{
    [Authorize]
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private static readonly Dictionary<string, string> _connections = new(); // ConnId -> UserId
        private static readonly Dictionary<string, string> _userToGroup = new(); // UserId -> GroupName
        private static readonly string AdminUserId = "18dee055-a58d-4044-a1fb-5b183dc326f9"; // Bunu gerçek admin user id ile değiştir

        private readonly ILoginService _loginService;

        public ChatHub(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = _loginService.UserId.ToString();

            _connections[Context.ConnectionId] = userId;

            // Her kullanıcıya özel bir oda: userId_admin
            if (userId != AdminUserId)
            {
                var groupName = GetPrivateGroupName(userId);
                _userToGroup[userId] = groupName;
                _userToGroup[AdminUserId] = groupName;

                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }
            else
            {
                // Admin bağlandığında tüm açık gruplara katılsın
                foreach (var group in _userToGroup.Values.Distinct())
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, group);
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out var userId))
            {
                _connections.Remove(Context.ConnectionId);

                if (_userToGroup.TryGetValue(userId, out var group))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            var userId = _loginService.UserId.ToString();

            if (_userToGroup.TryGetValue(userId, out var group))
            {
                await Clients.Group(group).SendAsync("ReceiveMessage", new
                {
                    Id = Guid.NewGuid(),
                    Message = message,
                    UserId = userId,
                    Timestamp = DateTime.UtcNow,
                    IsFromSupport = userId == AdminUserId
                });
            }
        }

        private string GetPrivateGroupName(string userId)
        {
            return $"chat_{userId}_admin";
        }
    }
}
