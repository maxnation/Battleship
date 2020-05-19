using System.Collections.Generic;

namespace Battleship.Models
{
    public class GameListViewModel
    {
        public IEnumerable<GameDescriptionViewModel> OthersFreeGames { get; set; }

        public IEnumerable<GameDescriptionViewModel> PendingGames { get; set; }

        public IEnumerable<GameDescriptionViewModel> PlayerFreeGames { get; set; }
    }
}
