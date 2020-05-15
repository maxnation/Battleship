using System.Collections.Generic;

namespace Battleship.Domain.Core
{
    public class Game
    {
        public int Id { get; set; }

        public GameState State { get; set; }
        
        public virtual ICollection<Player> Players { get; set; }

        public Game()
        {
            Players = new List<Player>(2);
        }
    }
}