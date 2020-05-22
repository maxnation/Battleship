using Battleship.Domain.Core;
using Battleship.Domain.Interfaces;
using Battleship.Mapping;
using Battleship.Models;
using Battleship.Models.FieldCreation;
using Battleship.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IUnitOfWork uof;
        private readonly IGameService gameService;
        private static Dictionary<int, int> firstStepPlayers;

        static GameController()
        {
            firstStepPlayers = new Dictionary<int, int>();
        }

        public GameController(IUnitOfWork uof, IGameService gameService)
        {
            this.uof = uof;
            this.gameService = gameService;
        }

        [HttpGet]
        public IActionResult CreateGame()
        {
            Game game = gameService.CreateGame(User.Identity.Name);

            HttpContext.Session.SetInt32("gameId", game.Id);
            HttpContext.Session.SetInt32("playerId", game.Players.First().Id);

            return RedirectToAction("CreateField");
        }

        [HttpGet]
        public IActionResult CreateField()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateField([FromBody]  IEnumerable<CreateShipViewModel> model)
        {
            List<Ship> ships = new List<Ship>();

            foreach (var shipVM in model)
            {
                Ship ship = new Ship
                {
                    Size = (byte)shipVM.Size,
                    XP = (byte)shipVM.Size
                };

                foreach (var cellVM in shipVM.Cells)
                {
                    ship.Cells.Add(new Cell { LineNo = (byte)cellVM.LineNo, ColumnNo = (byte)cellVM.ColumnNo });
                }

                ships.Add(ship);
            }
            gameService.CreateField(HttpContext.Session.GetInt32("playerId").Value, ships);

            return Json(new { redirectToUrl = Url.Action("Index", "Game") });
        }

        public JsonResult GetGameData([FromBody] int id)
        {
            GameViewModel gameVM = GameMap.GetGameViewModel(uof.GameRepository, User.Identity.Name, id);
            Step lastStep = uof.StepRepository
                .GetQueryable().Where(s=>s.PlayerId == gameVM.Player.PlayerId || s.PlayerId == gameVM.Rival.PlayerId)
                .OrderByDescending(s => s.StepNo)
                .Take(1)
                .FirstOrDefault();

            if (lastStep != null)
            {
                if(lastStep.Hit == true)
                {
                    gameVM.NextTurnPlayerId = lastStep.PlayerId.Value;
                }
                else
                {
                    gameVM.NextTurnPlayerId = lastStep.PlayerId == gameVM.Player.PlayerId ?
                         gameVM.Rival.PlayerId : gameVM.Player.PlayerId;
                }
            }
            else
            {
                if (firstStepPlayers.ContainsKey(gameVM.GameId))
                {
                    gameVM.NextTurnPlayerId = firstStepPlayers[gameVM.GameId];
                }
                else
                {
                    int randomVal = new Random(Guid.NewGuid().GetHashCode()).Next(0, 2);
                    gameVM.NextTurnPlayerId = randomVal == 0 ? gameVM.Player.PlayerId : gameVM.Rival.PlayerId;
                    firstStepPlayers[gameVM.GameId] = gameVM.NextTurnPlayerId;
                    firstStepPlayers.Remove(gameVM.GameId);
                }                 
            }

            return Json(gameVM);
        }

        public IActionResult Game(int id)
        {
            ViewData["gameId"] = id;
            return View("Game");
        }

        [HttpGet]
        public IActionResult JoinGame(int id)
        {
            Game game = gameService.JoinGame(id, User.Identity.Name);

            int playerId = game.Players.First(p => p.User.Email == User.Identity.Name).Id;
            HttpContext.Session.SetInt32("gameId", id);
            HttpContext.Session.SetInt32("playerId", playerId);

            return RedirectToAction("CreateField");
        }

        public IActionResult Index()
        {
            string username = User.Identity.Name;

            IEnumerable<Game> activeGames = new List<Game>();
            IEnumerable<Game> userFreeGames = new List<Game>();
            IEnumerable<Game> othersFreeGames = new List<Game>();
            gameService.GetUserGamesList(username, out userFreeGames, out othersFreeGames, out activeGames);

            var userFreeGamesDescriptions = userFreeGames?
                .Select(g => new GameDescriptionViewModel
                {
                    GameState = g.State.GetConstantName(),
                    FirstPlayerUsername = g.Players.FirstOrDefault()?.User?.Email,
                    Id = g.Id,
                })
          ?.ToList();

            var otherFreeGamesDescriptions = othersFreeGames?
          .Select(g => new GameDescriptionViewModel
          {
              GameState = g.State.GetConstantName(),
              FirstPlayerUsername = g.Players.FirstOrDefault()?.User?.Email,
              Id = g.Id
          })
          ?.ToList();

            var activeGamesDescription = activeGames?
                  .Select(g => new GameDescriptionViewModel
                  {
                      Id = g.Id,
                      GameState = "Playing",
                      FirstPlayerUsername = username,
                      SecondPlayerUsername = g.Players
                      .FirstOrDefault(p => p.User.Email != username)?.User?.Email
                  })
                ?.ToList();

            GameListViewModel gListVM = new GameListViewModel
            {
                PlayerFreeGames = userFreeGamesDescriptions,
                PendingGames = activeGamesDescription,
                OthersFreeGames = otherFreeGamesDescriptions
            };
            return View(gListVM);
        }
    }
}