using Battleship.Domain.Core;
using Battleship.Domain.Interfaces;
using Battleship.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Battleship.Mapping
{
    public static class GameMap
    {
        public static IEnumerable<GameStatisticsViewModel> GetGameStatisticsViewModel(IEnumerable<Game> games)
        {
            List<GameStatisticsViewModel> gameVMs = new List<GameStatisticsViewModel>();

            foreach (var game in games)
            {
                Player winner = game.Players.First(p => p.IsWinner);
                var winnerShips = winner.Field.Cells
                    .Where(c => c.State == CellState.Hit || c.State == CellState.Occupied)
                    .Select(c => c.Ship)
                    .Distinct(new ShipsEqualityComparer())
                    .ToList();

                GameStatisticsViewModel gameStatisticsVM = new GameStatisticsViewModel
                {
                    GameNo = game.Id,

                    WinnerUsername = winner.User.Email,

                    LoserUsername = game.Players.First(p => !p.IsWinner).User.Email,

                    NumberOfMovesInGame = (byte)game.Players.SelectMany(p => p.Steps).Count(),

                    WinnerShips = winnerShips
                    .Select(sh => new ShipStatisticsViewModel { Size = sh.Size, XP = sh.XP })
                    .ToArray()
                };

                gameStatisticsVM.TotalShipsSurvived = (byte)gameStatisticsVM.WinnerShips.Where(s => s.XP != 0).Count();
                gameStatisticsVM.TotalXP = (byte)gameStatisticsVM.WinnerShips.Select(s => (int)s.XP).Sum();

                gameVMs.Add(gameStatisticsVM);
            }

            return gameVMs;
        }

        public static GameViewModel GetGameViewModel(IRepository<Game> repository, string username, int gameId)
        {
            Game game = repository.GetQueryable()
                .Include(g => g.Players)
                .ThenInclude(p => p.Field)
                .ThenInclude(f => f.Cells)
                .ThenInclude(c => c.Ship)
                .Include(g => g.Players)
                .ThenInclude(p => p.User)
                .First(g => g.Id == gameId);

            var player = game.Players.FirstOrDefault(p => p.User.Email == username);
            var rival = game.Players.FirstOrDefault(p => p.User.Email != username);

            GameViewModel gameVM = new GameViewModel
            {
                GameId = game.Id,
                Player = new PlayerViewModel
                {
                    PlayerId = player.Id,
                    Username = player.User.Email,
                },
                Rival = new PlayerViewModel
                {
                    PlayerId = rival.Id,
                    Username = rival.User.Email,
                }
            };

            List<CellViewModel> playerCellViewModels = player.Field.Cells.Select(cell => new CellViewModel
            {
                Id = cell.Id,
                LineNo = cell.LineNo,
                ColumnNo = cell.ColumnNo,
                State = cell.State.GetConstantName()
            }).ToList();

            List<CellViewModel> rivalCellViewModels = rival.Field.Cells.Select(cell => new CellViewModel
            {
                Id = cell.Id,
                LineNo = cell.LineNo,
                ColumnNo = cell.ColumnNo,
                State = cell.State.GetConstantName()
            }).ToList();

            gameVM.Player.Field.Cells = playerCellViewModels;
            gameVM.Rival.Field.Cells = rivalCellViewModels;
            return gameVM;
        }
    }

    internal class ShipsEqualityComparer : IEqualityComparer<Ship>
    {
        public bool Equals([DisallowNull] Ship x, [DisallowNull] Ship y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] Ship obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}

