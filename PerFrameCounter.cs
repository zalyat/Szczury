using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Szczury
{
    internal static class PerFrameCounter
    {
        public static int renderingActionsInFrame;

        public static void Report()
        {
            Debug.WriteLine($"{renderingActionsInFrame}");
        }

        public static void Clear()
        {
            renderingActionsInFrame = 0;
        }
    }
}
