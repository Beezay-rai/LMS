using Microsoft.AspNetCore.SignalR;

namespace LMS.Services
{
    public class SignalrChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage",$"{Context.ConnectionId} has joined !");
        }
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
