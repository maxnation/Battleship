using Battleship.Domain.Core;

namespace Battleship.Services.Interfaces
{
    public interface IGameService
    {
        void CreateGame(int userId);

        void CreateField(int gameId, int playerId);

        void MakeMove(Step step, int gameId, int playerId);

        void JoinGame(int gameId, int userId);

        void JoinGame(int gameId, string username);

        void JoinGame(int gameId, ApplicationUser user);
    }
}
