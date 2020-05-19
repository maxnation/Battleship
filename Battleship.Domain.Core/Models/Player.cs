using System.Collections.Generic;

namespace Battleship.Domain.Core
{
    public class Player
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Game Game { get; set; }

        public int GameId { get; set; }

        public virtual Field Field { get; set; }

        public bool IsWinner { get; set; }

        public ICollection<Step> Steps { get; set; }

        public Player()
        {
            Steps = new List<Step>();
        }
    }
}