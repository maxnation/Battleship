namespace Battleship.Models
{
    public class StepViewModel
    {
        public int PlayerId;

        public int RivalId { get; set; }

        public int GameId { get; set; }

        public int LineNo { get; set; }

        public int ColumnNo { get; set; }
    }
}