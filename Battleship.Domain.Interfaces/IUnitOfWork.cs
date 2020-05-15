using Battleship.Domain.Core;

namespace Battleship.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<ApplicationUser> UserRepository { get; set; }

        IGenericRepository<Player> PlayerRepository { get; set; }

        IGenericRepository<Field> FieldRepository { get; set; }

        IGenericRepository<Game> GameRepository { get; set; }

        IGenericRepository<Cell> CellRepository { get; set; }

        IGenericRepository<Ship> ShipRepository { get; set; }

        IGenericRepository<Step> StepRepository { get; set; }
    }
}
