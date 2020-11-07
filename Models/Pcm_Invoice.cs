using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace currentweather.Models
{
    public partial class Pcm_Invoice
    {
        public Pcm_Invoice()
        {
            Payments = new HashSet<Pcm_Payment>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string InvoiceNr { get; set; }
        public string Text { get; set; }
        public decimal Price { get; set; }
        public string CurrencyNm { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? Sent { get; set; }
        public DateTime? Accepted { get; set; }
        public DateTime? Paid { get; set; }
        public DateTime? Cancelled { get; set; }
        public string CanceledReason { get; set; }
        public byte[] Sourcefile { get; set; }
        public string Url { get; set; }
        public byte[] Scan { get; set; }
        public string EMail { get; set; }
        public string PostAddr { get; set; }

        public virtual Pcm_Order Order { get; set; }
        public virtual ICollection<Pcm_Payment> Payments { get; set; }
    }
}
