namespace Battleship.Models
{
    public class GameStatisticsViewModel
    {
        public int GameNo { get; set; }

        public string WinnerUsername { get; set; }

        public string LoserUsername { get; set; }

        public byte TotalXP { get; set; }

        public byte TotalShipsSurvived { get; set; }

        public byte NumberOfMovesInGame { get; set; }

        public ShipStatisticsViewModel[] WinnerShips { get; set; }
    }
}