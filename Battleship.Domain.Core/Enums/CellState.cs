namespace Battleship.Domain.Core
{
    public enum CellState
    {
        Free = 0,
        Occupied = 1,
        Hit = 2,
        Miss = 3,
        OpenedAsNeighboring = 4,
    }
}