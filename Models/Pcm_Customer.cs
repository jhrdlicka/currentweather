using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class Pcm_Customer
    {
        public Pcm_Customer()
        {
            CalEvents = new HashSet<Pcm_CalEvent>();
            Orders = new HashSet<Pcm_Order>();
        }

        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string EMailCalendar { get; set; }
        public string EMailInvoice { get; set; }
        public string Phone { get; set; }
        public byte[] Photo { get; set; }
        public bool? Active { get; set; }
        public decimal? PriceSession { get; set; }
        public decimal? Price10sessions { get; set; }
        public string CurrencyNm { get; set; }
        public string AgreedConditions { get; set; }
        public string Note { get; set; }

        public virtual ICollection<Pcm_CalEvent> CalEvents { get; set; }
        public virtual ICollection<Pcm_Order> Orders { get; set; }
    }
}
