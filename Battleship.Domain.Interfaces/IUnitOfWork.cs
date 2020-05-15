using Battleship.Domain.Core;

namespace Battleship.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<ApplicationUser> UserRepository { get; set; }

        IRepository<Player> PlayerRepository { get; set; }

        IRepository<Field> FieldRepository { get; set; }

        IRepository<Game> GameRepository { get; set; }

        IRepository<Cell> CellRepository { get; set; }

        IRepository<Ship> ShipRepository { get; set; }

        IRepository<Step> StepRepository { get; set; }
    }
}
