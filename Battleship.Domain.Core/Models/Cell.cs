namespace Battleship.Domain.Core
{
    public class Cell
    {
        public int Id { get; set; }

        public byte LineNo { get; set; }

        public byte ColumnNo { get; set; }

        public int FieldId { get; set; }

        public virtual Field Field { get; set; }

        public CellState State { get; set; }

        public int? ShipId { get; set; }

        public virtual Ship Ship { get; set; }
    }
}