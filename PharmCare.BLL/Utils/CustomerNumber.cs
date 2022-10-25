using System;
using System.Collections.Generic;
using System.Text;

namespace PharmCare.BLL.Utils
{
    public static class CustomerNumber
    {
        public static string GenerateUniqueNumber()
        {
            string numbers = "543211";
            string characters = numbers;
            int length = 5;
            string id = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (id.IndexOf(character) != -1);
                id += character;
            }

            return id;
        }
    }
}
