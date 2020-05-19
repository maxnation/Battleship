using Battleship.Domain.Core;
using Battleship.Domain.Interfaces;
using Battleship.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Infrastructure.Business
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork unitOfWork;

        public GameService(IUnitOfWork uof)
        {
            this.unitOfWork = uof;
        }

        public Field CreateField(int playerId, IEnumerable<Ship> ships)
        {
            throw new NotImplementedException();
        }

        public Game CreateGame(int userId)
        {
            throw new NotImplementedException();
        }

        public Game CreateGame(string username)
        {
            throw new NotImplementedException();
        }

        public Game CreateGame(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        private Player CreatePlayer(int userId, int gameId)
        {
            throw new NotImplementedException();
        }

        private Player CreatePlayer(string username, int gameId)
        {
            throw new NotImplementedException();
        }

        private Player CreatePlayer(ApplicationUser user, int gameId)
        {
            throw new NotImplementedException();
        }

        public Game JoinGame(int gameId, int userId)
        {
            throw new NotImplementedException();
        }

        public Game JoinGame(int gameId, string username)
        {
            throw new NotImplementedException();
        }

        public Game JoinGame(int gameId, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public bool MakeMove(int lineNo, int columnNo, int playerId, int rivalId)
        {
            throw new NotImplementedException();
        }

        public void GetUserGamesList(string username,
            out IEnumerable<Game> userFreeGames, out IEnumerable<Game> othersFreeGames, out IEnumerable<Game> activeGames)
        {
            throw new NotImplementedException();
        }
    }
}