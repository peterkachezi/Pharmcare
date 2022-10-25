using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmCare.BLL.Utils
{
    public static class TransactionNumber
    {
        public static string GetNumber()
        {
            // Keep this _r as a member, not local
            var _r = new Random();

            // Gen a random number
            int rand = _r.Next(1, 10000);

            // Get the "2016-" prefix
            string yearPrefix = (DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond) + "-" + rand;

            // Remove the first 2 digits of the year prefix, now it is "16-"
            yearPrefix = yearPrefix.Substring(2);

            // Put the year prefix together with the random number into the textbox
            var orderNumber = yearPrefix + rand;

            rand = _r.Next(1, 10000);

            var customerNumber = yearPrefix + rand;

            return customerNumber;
        }
    }
}
