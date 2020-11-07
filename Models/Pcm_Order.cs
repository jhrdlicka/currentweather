using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace currentweather.Models
{
    public partial class Pcm_Order
    {
        public Pcm_Order()
        {
            Invoices = new HashSet<Pcm_Invoice>();
            OrderSessions = new HashSet<Pcm_OrderSession>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int? Sessions { get; set; }
        public long CustomerId { get; set; }
        public bool XFullyScheduled { get; set; }
        public bool XInvoiced { get; set; }
        public string Invoicetext { get; set; }
        public decimal? Price { get; set; }
        public string CurrencyNm { get; set; }

        public virtual Pcm_Customer Customer { get; set; }
        public virtual ICollection<Pcm_Invoice> Invoices { get; set; }
        public virtual ICollection<Pcm_OrderSession> OrderSessions { get; set; }
    }
}
