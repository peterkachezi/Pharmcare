
using System.ComponentModel;

namespace PharmCare.BLL.Utils
{
    public class Enumerations
    {
        public enum PaymentStatus
        {
            [Description("Pending")]
            Pending = 0,
            [Description("Paid")]
            Paid = 1,

        }


    }
}
