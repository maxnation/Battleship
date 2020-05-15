using System.Collections.Generic;

namespace Battleship.Domain.Core
{
    public class Ship
    {
        public int Id { get; set; }

        public byte Size { get; set; }

        public byte XP { get; set; }

        public virtual ICollection<Cell> Cells { get; set; }

        public Ship()
        {
            Cells = new List<Cell>();
        }
    }
}