using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class Pcm_OrderSession
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Invoicetext { get; set; }
        public decimal? Price { get; set; }
        public string CurrencyNm { get; set; }
        public long? CaleventId { get; set; }

        public virtual Pcm_CalEvent Calevent { get; set; }
        public virtual Pcm_Order Order { get; set; }
    }
}
