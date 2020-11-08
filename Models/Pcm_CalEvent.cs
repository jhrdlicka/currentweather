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
        public TimeSpan? duration { get; set; }
        public string title { get; set; }
        public long? customerid { get; set; }
        public string gcalid { get; set; }
        public string gcalstatus { get; set; }
        public string gcallink { get; set; }
        public DateTime? gcalcreated { get; set; }
        public DateTime? gcalupdated { get; set; }
        public string gcalsummary { get; set; }
        public string gcaldescription { get; set; }
        public string gcallocation { get; set; }
        public DateTime? gcalstart { get; set; }
        public DateTime? gcalend { get; set; }
        public string gcaljson { get; set; }
        public string xordered { get; set; }
        public string invoicetext { get; set; }
        public decimal? price { get; set; }
        public string currencynm { get; set; }
        public DateTime? canceled { get; set; }
        public string canceledreason { get; set; }

        public virtual pcm_customer customer { get; set; }
        public virtual ICollection<pcm_ordersession> pcm_ordersession { get; set; }
    }
}
