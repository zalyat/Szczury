using System;
using Szczury.Core;

namespace Szczury
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameCore())
                game.Run();
        }
    }
}
