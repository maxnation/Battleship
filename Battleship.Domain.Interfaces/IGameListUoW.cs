using Battleship.Domain.Core;

namespace Battleship.Domain.Interfaces
{
    public interface IGameListUoF
    {
        IGenericRepository<ApplicationUser> Users { get; set; }

        IGenericRepository<Player> Players { get; set; }

        IGenericRepository<Game> Games { get; set; }
    }
}