using System;

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

    public static class CellStateHelper
    {
        public static string GetConstantName(this CellState state)
        {
            return Enum.GetName(typeof(CellState), state);
        }
    }
}