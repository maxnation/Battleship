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
            ApplicationUser user = unitOfWork.UserRepository.FindById(userId);
            return CreateGame(user);
        }

        public Game CreateGame(string username)
        {
            ApplicationUser user = unitOfWork.UserRepository.FirstOrDefault(u => u.Email == username);
            return CreateGame(user);
        }

        public Game CreateGame(ApplicationUser user)
        {
            Game game = new Game { State = GameState.Created | GameState.WaitingForSecondPlayer };
            unitOfWork.GameRepository.Create(game);
            this.CreatePlayer(user, game.Id);
            return game;
        }

        private Player CreatePlayer(int userId, int gameId)
        {
            ApplicationUser user = unitOfWork.UserRepository.FindById(userId);
            return CreatePlayer(user, gameId);
        }

        private Player CreatePlayer(string username, int gameId)
        {
            ApplicationUser user = unitOfWork.UserRepository.Get(u => u.Email == username).First();
            return CreatePlayer(user, gameId);
        }

        private Player CreatePlayer(ApplicationUser user, int gameId)
        {
            Player player = new Player
            {
                UserId = user.Id,
                GameId = gameId
            };
            user.Players.Add(player);
            unitOfWork.UserRepository.Update(user);
            return player;
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