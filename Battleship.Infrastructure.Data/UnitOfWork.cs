using Battleship.Domain.Core;
using Battleship.Domain.Interfaces;

namespace Battleship.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        private IRepository<ApplicationUser> userRepository;
        public IRepository<ApplicationUser> UserRepository { get => userRepository ?? (userRepository = new Repository<ApplicationUser>(context)); }

        private IRepository<Player> playerRepository;
        public IRepository<Player> PlayerRepository { get => playerRepository ?? (playerRepository = new Repository<Player>(context)); }

        private IRepository<Field> fieldRepository;
        public IRepository<Field> FieldRepository { get => fieldRepository ?? (fieldRepository = new Repository<Field>(context)); }

        private IRepository<Game> gameRepository;
        public IRepository<Game> GameRepository { get => gameRepository ?? (gameRepository = new Repository<Game>(context)); }

        private IRepository<Cell> cellRepository;
        public IRepository<Cell> CellRepository { get => cellRepository ?? (cellRepository = new Repository<Cell>(context)); }

        private IRepository<Ship> shipRepository;
        public IRepository<Ship> ShipRepository { get => shipRepository ?? (shipRepository = new Repository<Ship>(context)); }

        private IRepository<Step> stepRepository;
        public IRepository<Step> StepRepository { get => stepRepository ?? (stepRepository = new Repository<Step>(context)); }
    }
}