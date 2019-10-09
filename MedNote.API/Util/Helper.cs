using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MedNote.API.Util
{
    public class Helper
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        /// <summary>
        /// Formata o caminho percorrido.
        /// </summary>
        public static string StackTraceFormat(StackTrace stackTrace, string stopFrame)
        {
            StringBuilder sb = new StringBuilder();

            StackFrame[] stackFrames = stackTrace.GetFrames().ToArray();

            for (int i = stackFrames.Count() - 1; i >= 0; i--)
            {
                if (stopFrame != "" && stopFrame == stackFrames[i].GetMethod().Name)
                    sb.Clear();
                else
                    sb.AppendFormat(i == 0 ? "{0}" : "{0}->", stackFrames[i].GetMethod().Name);
            }

            return sb.ToString();
        }

        public static string montaNumeroPOS(string numero, int quantidade)
        {
            char pad = '0';
            string numeroPOS = "";

            numeroPOS = "VIX" + DateTime.Now.Year + "-" + numero.PadLeft(5, pad) + "/" + quantidade.ToString().PadLeft(2, pad);
            return numeroPOS;
        }

        public static string gerarSenhaCPF(string cpf)
        {
            string primeiro = cpf.Substring(0, 4);
            string segundo = cpf.Substring(9, 2);
            return primeiro + segundo;
        }
    }
}