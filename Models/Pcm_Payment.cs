using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class pcm_payment
    {
        public long id { get; set; }
        public long? invoiceid { get; set; }
        public decimal amount { get; set; }
        public string currencynm { get; set; }
        public DateTime? date { get; set; }
        public string typenm { get; set; }
        public string referencenr { get; set; }
        public string paidby { get; set; }
        public string description { get; set; }
        public string fromaccountnr { get; set; }

        public virtual pcm_invoice invoice { get; set; }
    }
}
