using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace Practice
{
    public class chatHub : Hub
    {

        public async Task JoinRoom(string roomName, string userName)
        {
            // Add this connection to a SignalR "Group" named after the room
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

            // Notify everyone already in the room that someone joined
            await Clients.Group(roomName).SendAsync(
                "ReceiveMessage", "System", $"{userName} has joined {roomName}");
        }

        // Called when a user wants to leave a room
        public async Task LeaveRoom(string roomName, string userName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

            await Clients.Group(roomName).SendAsync(
                "ReceiveMessage", "System", $"{userName} has left {roomName}");
        }

        // Called when a user sends a message — now scoped to their room
        public async Task SendMessageToRoom(string roomName, string user, string message)
        {
            // Only people in THIS group/room receive it
            await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // SignalR automatically removes the connection from all groups,
            // but you can still notify others if you're tracking room membership yourself
            await base.OnDisconnectedAsync(exception);
        }
    }
    
}
