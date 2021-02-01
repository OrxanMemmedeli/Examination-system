using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAndQuiz.Tools
{
    public static class CodeGenerator
    {
        public static string Code()
        {
            string code = "";
            //string[] simvol = new string[] { "Q", "q", "W", "w", "E", "e", "R", "r", "T", "t", "Y", "y", "U", "u", "I", "i", "O", "o", "P", "p", "A", "a", "S", "s", "D", "d", "F", "f", "G", "g", "H", "h", "J", "j", "K", "k", "L", "L", "Z", "z", "X", "x", "C", "c", "V", "v", "B", "b", "N", "n", "M", "m", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] simvol = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                code += simvol[random.Next(9)];
            }
            return code;
        }
    }
}