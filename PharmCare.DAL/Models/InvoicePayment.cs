using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmCare.DAL.Models
{
    public partial class InvoicePayment
    {
        public Guid Id { get; set; }
        public decimal AmountPayable { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal PaymentMode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
