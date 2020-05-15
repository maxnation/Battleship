namespace Battleship.Domain.Core
{
    public class Cell
    {
        public int Id { get; set; }

        public string LineNo { get; set; }

        public string ColumnNo { get; set; }

        public int FieldId { get; set; }

        public virtual Field Field { get; set; }

        public State State { get; set; }

        public int ShipId { get; set; }

        public virtual Ship Ship { get; set; }
    }
}