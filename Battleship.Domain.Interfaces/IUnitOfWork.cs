using Battleship.Domain.Core;

namespace Battleship.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<ApplicationUser> UserRepository { get; }

        IRepository<Player> PlayerRepository { get; }

        IRepository<Field> FieldRepository { get; }

        IRepository<Game> GameRepository { get; }

        IRepository<Cell> CellRepository { get; }

        IRepository<Ship> ShipRepository { get; }

        IRepository<Step> StepRepository { get; }
    }
}