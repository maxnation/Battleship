using Battleship.Domain.Interfaces;
using Battleship.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Battleship
{
    public class GameHub : Hub
    {
        private readonly IGameService gameService;
        private readonly IUnitOfWork unitOfWork;

        public GameHub(IGameService gameService, IUnitOfWork unitOfWork)
        {
            this.gameService = gameService;
            this.unitOfWork = unitOfWork;
        }

        public async Task JoinGame(int gameId, string username)
        {
            string groupName = gameId.ToString();
            await this.Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await this.Clients.Group(groupName).SendAsync("joinGameLog", username, gameId);
        }
    }
}