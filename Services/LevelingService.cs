using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveLingo.Services
{
    // XP = 100 * L^1.5
    public static class LevelingService
    {
        /// <summary>
        /// Total XP needed from 0 to reach a specific level threshold.
        /// Level 1 = 0 XP, Level 2 = 100 XP, Level 3 = 282 XP, etc.
        /// </summary>
        public static int TotalXpForLevel(int level)
        {
            if (level <= 1) return 0;
            return (int)Math.Round(100.0 * Math.Pow(level - 1, 1.5));
        }

        /// <summary>
        /// Calculates player level from current total XP.
        /// Inverse formula: Level = floor((XP / 100) ^ (2/3)) + 1
        /// </summary>
        public static int CalculateCurrentLevel(int xp)
        {
            if (xp <= 0) return 1;

            int level = (int)Math.Floor(Math.Pow((double)xp / 100.0, 2.0 / 3.0)) + 1;
            return level;
        }

        /// <summary>
        /// Calculates additional XP needed to go from currentLevel to currentLevel + 1.
        /// </summary>
        public static int CalculateRequiredXP(int currentLevel)
        {
            if (currentLevel < 1) currentLevel = 1;

            return TotalXpForLevel(currentLevel + 1) - TotalXpForLevel(currentLevel);
        }

        /// <summary>
        /// XP progress towards the next level (progress bar value).
        /// </summary>
        public static int CalculateXpProgress(int xp)
        {
            int currentLevel = CalculateCurrentLevel(xp);
            int xpAtStartOfLevel = TotalXpForLevel(currentLevel);

            return Math.Max(0, xp - xpAtStartOfLevel);
        }
    }
}