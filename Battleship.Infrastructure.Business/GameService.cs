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
            else if (game.State == GameState.WaitingForBothFieldsCreated)
            {
                game.State = GameState.Playing;
            }
            unitOfWork.GameRepository.Update(game);
            var occupiedCells = ships.SelectMany(s => s.Cells);

            foreach (var c in occupiedCells)
            {
                c.State = CellState.Occupied;
                c.FieldId = field.Id;
            }

            unitOfWork.ShipRepository.CreateRange(ships);

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

            unitOfWork.CellRepository.CreateRange(freeCells);

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

        public bool MakeMove(int lineNo, int columnNo, int playerId, int rivalId,
            out bool isHit, out bool isGameOver)
        {
            isGameOver = false;
            
            Cell cell = unitOfWork.CellRepository
                .GetQueryable().Include(c => c.Ship)
                .FirstOrDefault(c => c.LineNo == lineNo && c.ColumnNo == columnNo
                 && c.Field.PlayerId == rivalId);

            this.ChangeCellState(cell, out isHit, rivalId, playerId, out isGameOver);
            this.SaveStep(playerId, rivalId, cell.Id, isHit);
          
            return isHit;
        }

        private void CheckRivalXP(int rivalId, int playerId, out bool isGameOver)
        {
            isGameOver = false;
            int totalXP;
            this.GetRivalXP(rivalId, out totalXP);
            
            if (totalXP == 0)
            {
                Player player = unitOfWork.PlayerRepository.
                    GetQueryable().Include(p=>p.Game)
                    .FirstOrDefault(p=>p.Id == playerId);
                
                player.IsWinner = true;
                player.Game.State = GameState.Finished;
                unitOfWork.PlayerRepository.Update(player);               
                
                isGameOver = true;
            }
        }

        private void ChangeCellState(Cell cell, out bool isHit, int rivalId, int playerId, out bool isGameOver)
        {
            isHit = false;
            isGameOver = false;
            if (cell.State == CellState.Free)
            {
                cell.State = CellState.Miss;
            }
            else if (cell.State == CellState.Occupied)
            {
                cell.State = CellState.Hit;
                isHit = true;
                this.SubtractShipXP(cell.Ship);
                this.CheckRivalXP(rivalId, playerId, out isGameOver);
            }
            unitOfWork.CellRepository.Update(cell);
        }

        private void GetRivalXP(int rivalId, out int totalXP)
        {
            var ships = unitOfWork.FieldRepository.GetQueryable()
                  .Include(f => f.Cells).ThenInclude(c => c.Ship)
                  .FirstOrDefault(f => f.PlayerId == rivalId)
                  .Cells.Select(c => c.Ship).Distinct();
            totalXP = ships.Sum(s => s!=null? s.XP : 0);
         }

        private void SaveStep(int playerId, int rivalId, int cellId, bool isHit)
        {
            int lastStepNo;
            var query = unitOfWork.StepRepository
                .GetQueryable()
                .Where(s => s.PlayerId == playerId || s.PlayerId == rivalId);

            if (query.Any())
            {
                lastStepNo = query.Max(s => s.StepNo);
            }
            else
            {
                lastStepNo = 0;
            }

            Step step = new Step
            {
                CellId = cellId,
                PlayerId = playerId,
                StepNo = ++lastStepNo,
                Hit = isHit
            };

            unitOfWork.StepRepository.Create(step);
        }

        private void SubtractShipXP(Ship ship)
        {
            ship.XP--;
            unitOfWork.ShipRepository.Update(ship);
        }

        public void GetUserGamesList(string username,
             out IEnumerable<Game> userFreeGames, out IEnumerable<Game> othersFreeGames, out IEnumerable<Game> activeGames)
        {
            var user = unitOfWork.UserRepository.GetQueryable()
                       .Where(u => u.Email == username)
                           .Include(u => u.Players)
                           .ThenInclude(p => p.Game)
                           .ThenInclude(g => g.Players)
                           .ThenInclude(p => p.User)
                           .First();

            // Games of current User without second player
            userFreeGames = user.Players
               .Select(p => p.Game)
               .Where(g => g.State == (GameState.Created | GameState.WaitingForSecondPlayer) ||
               g.State == (GameState.WaitingForSecondPlayer | GameState.WaitingForBothFieldsCreated))
               .ToList();

            // Games of other Users without second player
            othersFreeGames = unitOfWork.GameRepository.GetQueryable()
               .Where(g => g.State == (GameState.Created | GameState.WaitingForSecondPlayer) ||
               g.State == (GameState.WaitingForSecondPlayer | GameState.WaitingForBothFieldsCreated))
               .Where(g => g.Players.FirstOrDefault().User.Id != user.Id).Include(g => g.Players).ThenInclude(p => p.User)
               .ToList();

            // Active games of current User        
            activeGames = user.Players
                .Select(p => p.Game)
                .Where(g => g.State == GameState.Playing)?
                .ToList();
        }
    }
}