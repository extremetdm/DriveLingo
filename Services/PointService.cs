using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveLingo.Services
{
    public static class PointService
    {
        public static int CalculateForQuiz(int totalQuestions)
        {
            return 5 * totalQuestions;
        }
    }
}