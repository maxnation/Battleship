using System;

namespace Battleship.Domain.Core
{
    [Flags]
    public enum GameState
    {
        Created = 1,
        WaitingForSecondPlayer = 2,
        WaitingForBothFieldsCreated = 4,
        Playing = 8,
        Finished = 16
    }

    public static class GameStateHelper
    {
        public static string GetConstantName(this GameState state)
        {
            return Enum.GetName(typeof(GameState), state);
        }
    }
}