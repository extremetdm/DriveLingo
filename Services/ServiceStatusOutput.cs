using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveLingo.Services
{
    public struct ServiceStatusOutput
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ServiceStatusOutput(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}