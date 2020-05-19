namespace Battleship.Models
{
    public class GameDescriptionViewModel
    {
        public int Id { get; set; }

        public string FirstPlayerUsername { get; set; }

        public string SecondPlayerUsername { get; set; }

        public string GameState { get; set; }
    }
}
