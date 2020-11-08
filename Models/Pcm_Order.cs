using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class pcm_order
    {
        public pcm_order()
        {
            pcm_invoice = new HashSet<pcm_invoice>();
            pcm_ordersession = new HashSet<pcm_ordersession>();
        }

        public long id { get; set; }
        public int? sessions { get; set; }
        public long customerid { get; set; }
        public bool xfullyscheduled { get; set; }
        public bool xinvoiced { get; set; }
        public string invoicetext { get; set; }
        public decimal? price { get; set; }
        public string currencynm { get; set; }

        public virtual pcm_customer customer { get; set; }
        public virtual ICollection<pcm_invoice> pcm_invoice { get; set; }
        public virtual ICollection<pcm_ordersession> pcm_ordersession { get; set; }
    }
}
