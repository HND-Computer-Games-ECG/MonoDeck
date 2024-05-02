using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDeck
{
    public static class GameSettings
    {
        public static readonly float UIFadeDuration = 5;

        public static readonly int StartingHandSize = 5;
        public static readonly int HandSizeMax = 13;
        public static readonly int[] HandRewards = { 1, 2, 3, 5, 7, 10 };
        public static readonly int HandRefillSize = 4;

        public static readonly int PeepHPMax = 16;
        public static readonly int PeepStartingHP = 12;
    }
}
