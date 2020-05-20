namespace Battleship.Models
{
    public class PlayerViewModel
    {
        public int PlayerId { get; set; }

        public string Username { get; set; }

        public FieldViewModel Field { get; set; }

        public PlayerViewModel()
        {
            Field = new FieldViewModel();
        }
    }
}