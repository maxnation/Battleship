using Battleship.Domain.Core;
using Battleship.Domain.Interfaces;
using Battleship.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Mapping
{
    public static class GameMap
    {
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
                State = cell.State
            }).ToList();

            List<CellViewModel> rivalCellViewModels = rival.Field.Cells.Select(cell => new CellViewModel
            {
                Id = cell.Id,
                LineNo = cell.LineNo,
                ColumnNo = cell.ColumnNo,
                State = cell.State
            }).ToList();

            gameVM.Player.Field.Cells = playerCellViewModels;
            gameVM.Rival.Field.Cells = rivalCellViewModels;
            return gameVM;
        }
    }
}
