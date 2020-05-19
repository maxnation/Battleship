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
            Field field = new Field { PlayerId = playerId };
            unitOfWork.FieldRepository.Create(field);

            Game game = unitOfWork.GameRepository
                .GetQueryable()
                .Where(g => g.Players.Any(p => p.Id == playerId))
                .First();

            if (game.State == (GameState.Created | GameState.WaitingForSecondPlayer))
            {
                game.State = GameState.WaitingForSecondPlayer | GameState.WaitingForBothFieldsCreated;
            }
            else if (game.State == (GameState.WaitingForSecondPlayer | GameState.WaitingForBothFieldsCreated))
            {
                game.State = GameState.Playing;
            }
            var occupiedCells = ships.SelectMany(s => s.Cells);

            foreach (var c in occupiedCells)
            {
                c.State = CellState.Occupied;
                c.FieldId = field.Id;
            }

            unitOfWork.ShipRepository.BulkCreate(ships);

            var freeCells = new List<Cell>(80);

            for (byte line = 0; line < 10; line++)
            {
                for (byte column = 0; column < 10; column++)
                {
                    if (!occupiedCells.Any(c => c.LineNo == line && c.ColumnNo == column))
                    {
                        freeCells.Add(new Cell
                        {
                            LineNo = line,
                            ColumnNo = column,
                            FieldId = field.Id,
                            State = CellState.Free
                        });
                    }
                }
            }

            unitOfWork.CellRepository.BulkCreate(freeCells);

            return field;
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
            Player player = new Player { UserId = userId, GameId = gameId };
            unitOfWork.PlayerRepository.Create(player);

            Game game = unitOfWork.GameRepository.GetQueryable(g => g.Id == gameId)
                .Include(g => g.Players)
                .ThenInclude(p => p.UserId)
                .First();

            game.State = GameState.WaitingForBothFieldsCreated;
            unitOfWork.GameRepository.Update(game);

            return game;
        }

        public Game JoinGame(int gameId, string username)
        {
            int userId = unitOfWork.UserRepository
                .FirstOrDefault(u => u.Email == username).Id;

            return this.JoinGame(gameId, userId);
        }

        public Game JoinGame(int gameId, ApplicationUser user)
        {
            return this.JoinGame(gameId, user.Id);
        }

        public bool MakeMove(int lineNo, int columnNo, int playerId, int rivalId)
        {
            bool hit = false;

            int fieldId = unitOfWork.FieldRepository.Get(f => f.PlayerId == rivalId).First().Id;

            Cell cell = unitOfWork.CellRepository.FirstOrDefault(c => c.FieldId == fieldId && c.LineNo == lineNo && c.ColumnNo == columnNo);

            Step step = new Step { CellId = cell.Id, PlayerId = playerId };

            if (cell.State == CellState.Free)
            {
                cell.State = CellState.Miss;
                step.Hit = hit = false;
            }
            else if (cell.State == CellState.Occupied)
            {
                cell.State = CellState.Hit;
                step.Hit = hit = true;
            }

            unitOfWork.StepRepository.Create(step);
            unitOfWork.CellRepository.Update(cell);

            return hit;
        }

        public void GetUserGamesList(string username,
            out IEnumerable<Game> userFreeGames, out IEnumerable<Game> othersFreeGames, out IEnumerable<Game> activeGames)
        {
            throw new NotImplementedException();
        }
    }
}