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
    }
}