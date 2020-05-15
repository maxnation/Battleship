using Battleship.Domain.Core;

namespace Battleship.Domain.Interfaces
{
    public interface IContinueUoW
    {
        IGenericRepository<ApplicationUser> Users { get; set; }

        IGenericRepository<Player> Players { get; set; }

        IGenericRepository<Game> Games { get; set; }

        IGenericRepository<Field> Fields { get; set; }
    }
}
