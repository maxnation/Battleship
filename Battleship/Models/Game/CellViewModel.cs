using Battleship.Domain.Core;

namespace Battleship.Models
{
    public class CellViewModel
    {
        public int Id { get; set; }

        public int LineNo { get; set; }

        public int ColumnNo { get; set; }

        public string State { get; set; }
    }
}