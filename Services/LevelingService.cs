using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveLingo.Services
{
    // XP = 100 * L^1.5
    public static class LevelingService
    {
        public static int CalculateRequiredXP(int currentLevel)
        {
            return (int)(100 * Math.Pow(currentLevel + 1, 1.5) - Math.Pow(currentLevel, 1.5));
        }

        public static int CalculateCurrentLevel(int xp)
        {
            return (int)Math.Pow((double)xp / 100, 2 / 3);
        }
    }
}