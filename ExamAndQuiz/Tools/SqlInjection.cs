using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAndQuiz.Tools
{
    public class SqlInjection
    {
        public static string Protect(string value)
        {
            string result = value.Replace("'", "");
            result = result.Replace("<", "");
            result = result.Replace(">", "");
            return result;
        }
    }
}