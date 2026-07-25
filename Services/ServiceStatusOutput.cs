using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveLingo.Services
{
    public struct ServiceStatusOutput
    {
        public bool Success { get; }
        public string Message { get; }
        public ICollection<Achievement> UnlockedAchievements { get; }

        public ServiceStatusOutput(bool success, string message)
            : this(success, message, null)
        {
        }

        public ServiceStatusOutput(bool success, string message, ICollection<Achievement> unlockedAchievements)
        {
            Success = success;
            Message = message;
            UnlockedAchievements = unlockedAchievements ?? new HashSet<Achievement>();
        }

        public static ServiceStatusOutput error(string message)
        {
            return new ServiceStatusOutput(false, message);
        }

        public static ServiceStatusOutput success(string message)
        {
            return success(message, null);
        }

        public static ServiceStatusOutput success(string message, ICollection<Achievement> unlockedAchievements)
        {
            return new ServiceStatusOutput(false, message, unlockedAchievements);
        }
    }
}