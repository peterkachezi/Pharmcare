using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmCare.BLL.Utils
{
    public static class RandomNumbers
    {
        public static  string Generate_Number()
        {
            char[] charArr = "0123456789".ToCharArray();

            string strrandom = string.Empty;

            Random objran = new Random();

            for (int i = 0; i < 4; i++)
            {
                //It will not allow Repetation of Characters
                int pos = objran.Next(1, charArr.Length);

                if (!strrandom.Contains(charArr.GetValue(pos).ToString())) strrandom += charArr.GetValue(pos);

                else i--;
            }
            return strrandom;
        }
    }
}
