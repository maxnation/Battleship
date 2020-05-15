using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleship.Domain.Core
{
    public enum State
    {
        Free,
        Occupied,
        Hit,
        Miss,
        OpenedAsNeighboring = Miss
    }
}