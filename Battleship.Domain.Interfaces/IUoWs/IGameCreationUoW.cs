using Battleship.Domain.Core;

namespace Battleship.Domain.Interfaces
{
    public interface IGameCreationUoW
    {
        IGenericRepository<Player> Players { get; set; }

        IGenericRepository<Game> Games { get; set; }
    }
}
