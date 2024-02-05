using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Szczury
{
    public static class GameState
    {
        public static IState currentState;
        public static ContentManager content;
        public static void ChangeState(IState newState)
        {
            currentState = newState;
            currentState.Initialize(content);
            currentState.Start();
        }
    }
}
