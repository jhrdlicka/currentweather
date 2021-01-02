using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class pcm_ordersession
    {
        public long id { get; set; }
        public long orderid { get; set; }
        public string invoicetext { get; set; }
        public decimal? price { get; set; }
        public long? caleventid { get; set; }
        public decimal? rate { get; set; }

    // computed columns
        public int? xord { get; set; }

        public virtual pcm_calevent calevent { get; set; }
        public virtual pcm_order order { get; set; }
    }
}
