using Battleship.Domain.Core;
using System.Collections.Generic;

namespace Battleship.Services.Interfaces
{
    public interface IGameService
    {
        Game CreateGame(int userId);

        Game CreateGame(string username);

        Game CreateGame(ApplicationUser user);

        Field CreateField(int playerId, IEnumerable<Ship> ships);

        bool MakeMove(int lineNo, int columnNo, int playerId, int rivalId, out bool isHit, out bool isGameOver);

        Game JoinGame(int gameId, int userId);

        Game JoinGame(int gameId, string username);

        Game JoinGame(int gameId, ApplicationUser user);

        void GetUserGamesList(string username, out IEnumerable<Game> userFreeGames, out IEnumerable<Game> othersFreeGames, out IEnumerable<Game> activeGames);

        List<Game> GetGameDataForStatistics();
    }
}