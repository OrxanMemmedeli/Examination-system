using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ExamAndQuiz.Tools
{
    public class Encryption
    {
        public static string Encrypt(string value)
        {
            int value1 = Convert.ToInt32(value);
            value1 = value1 * 338 * 542;
            value1 += 67345;
            byte[] request = ASCIIEncoding.ASCII.GetBytes("|/-+!@#$%^" + value1.ToString() + "|/-+!@#$%^");
            string respons = Convert.ToBase64String(request);
            return respons;
        }

        public static string Decrypt(string value)
        {
            byte[] request = Convert.FromBase64String(value);
            string respons = ASCIIEncoding.ASCII.GetString(request);
            respons = respons.Replace("|/-+!@#$%^", "");
            int value1 = Convert.ToInt32(respons);
            value1 -= 67345;
            value1 = value1 / 338 / 542;
            respons = value1.ToString();
            return respons;
        }

        public static string PassEncrypt(string value)
        {
            byte[] request = ASCIIEncoding.ASCII.GetBytes(value);
            string respons = Convert.ToBase64String(request);
            return respons;
        }

        public static string PassDecrypt(string value)
        {
            byte[] request = Convert.FromBase64String(value);
            string respons = ASCIIEncoding.ASCII.GetString(request);
            return respons;
        }
    }
}