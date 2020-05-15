namespace Battleship.Domain.Core
{
    public class Step
    {
        public int Id { get; set; }

        public int? PlayerId { get; set; }

        public Player Player { get; set; }

        public int CellId { get; set; }

        public Cell Cell { get; set; }

        public bool Hit { get; set; }
    }
}