using Battleship.Domain.Core;
using Battleship.Domain.Interfaces;
using Battleship.Models;
using Battleship.Models.FieldCreation;
using Battleship.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IUnitOfWork uof;
        private readonly IGameService gameService;

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

        [HttpGet]
        public IActionResult JoinGame(int id)
        {
            Game game = gameService.JoinGame(id, User.Identity.Name);

            int playerId = game.Players.First(p => p.User.Email == User.Identity.Name).Id;
            HttpContext.Session.SetInt32("gameId", id);
            HttpContext.Session.SetInt32("playerId", playerId);

            return RedirectToAction("CreateField");
        }
    }
}