using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class pcm_calevent
    {
        public pcm_calevent()
        {
            pcm_ordersession = new HashSet<pcm_ordersession>();
        }

        public long id { get; set; }
        public DateTime? start { get; set; }
        public long? duration { get; set; }
        public string title { get; set; }
        public long? customerid { get; set; }
        public string gcalid { get; set; }
        public string gcaljson { get; set; }
        public string invoicetext { get; set; }
        public DateTime? canceled { get; set; }
        public string canceledreason { get; set; }
        public decimal? units { get; set; }

        public virtual pcm_customer customer { get; set; }
        public virtual ICollection<pcm_ordersession> pcm_ordersession { get; set; }
    }
}
