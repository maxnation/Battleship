using System.Collections.Generic;

namespace Battleship.Domain.Core
{
    public class Field
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public virtual ICollection<Cell> Cells { get; set; }

        public Field()
        {
            Cells = new List<Cell>();
        }
    }
}