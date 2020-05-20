using Battleship.Domain.Core;

namespace Battleship.Models
{
    public class CellViewModel
    {
        public int LineNo { get; set; }

        public int ColumnNo { get; set; }

        public CellState State { get; set; }
    }
}