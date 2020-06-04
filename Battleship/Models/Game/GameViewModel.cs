namespace Battleship.Models
{
    public class GameViewModel
    {
        public int GameId { get; set; }

        public PlayerViewModel Player { get; set; }

        public PlayerViewModel Rival { get; set; }

        public int NextTurnPlayerId { get; set; } // Для вывода юзернейма пользователя, чья очередь ходить

        public GameViewModel()
        {
            Player = new PlayerViewModel();
            Rival = new PlayerViewModel();
        }
    }
}