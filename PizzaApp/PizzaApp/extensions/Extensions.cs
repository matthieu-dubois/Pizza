using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaApp.extensions
{
     public static class StringExtensions
    {
        // en ajoutant le mot this nous allons ajouter cette methode dans les methodes de bases ToString()
        public static string PremiereLettreMaj(this string str)
        {
            if (String.IsNullOrEmpty(str))

            {
                return str;
            }

            string ret = str.ToLower();

            ret = ret.Substring(0, 1).ToUpper() + ret.Substring(1, ret.Length - 1);

            return ret;
        }
    }
}
