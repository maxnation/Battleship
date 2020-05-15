using System;

namespace Battleship.Domain.Core
{
    public enum GameState
    {
        Created,
        WaitingForSecondPlayer,
        WaitingForFieldsCreating,
        Playing,
        Finished
    }

    public static class GameStateHelper
    {
        public static string GetConstantName(this GameState state)
        {
            return Enum.GetName(typeof(GameState), state);
        }
    }
}