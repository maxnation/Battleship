using System.Collections.Generic;

namespace Battleship.Domain.Core
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public ApplicationUser()
        {
            Players = new List<Player>();
        }
    }
}