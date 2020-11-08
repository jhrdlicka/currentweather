using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class pcm_invoice
    {
        public pcm_invoice()
        {
            pcm_payment = new HashSet<pcm_payment>();
        }

        public long id { get; set; }
        public long orderid { get; set; }
        public string invoicenr { get; set; }
        public string text { get; set; }
        public decimal price { get; set; }
        public string currencynm { get; set; }
        public DateTime? eventdate { get; set; }
        public DateTime? sent { get; set; }
        public DateTime? accepted { get; set; }
        public DateTime? paid { get; set; }
        public DateTime? canceled { get; set; }
        public string canceledreason { get; set; }
        public byte[] sourcefile { get; set; }
        public string link { get; set; }
        public byte[] scan { get; set; }
        public string email { get; set; }
        public string postaddr { get; set; }

        public virtual pcm_order order { get; set; }
        public virtual ICollection<pcm_payment> pcm_payment { get; set; }
    }
}
