using DriveLingo.Database;
using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveLingo.Services
{
    public static class ModuleService
    {
        public static ICollection<Module> GetModules()
        {
            using (var db = new AppDbContext())
            {
                return db.Modules.ToList();
            }
        }

        //public static ServiceStatusOutput AddModule(string name, string description)
        //{

        //}

        //public static ServiceStatusOutput EditModule(int string name, string description)
        //{

        //}
    }
}