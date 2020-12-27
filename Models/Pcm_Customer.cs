using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class pcm_customer
    {
        public pcm_customer()
        {
            pcm_calevent = new HashSet<pcm_calevent>();
            pcm_order = new HashSet<pcm_order>();
        }

        public long id { get; set; }
        public string name { get; set; }
        public string emailcalendar { get; set; }
        public string emailinvoice { get; set; }
        public string postaddr { get; set; }
        public string phone { get; set; }
        public bool? active { get; set; }
        public decimal? pricesession { get; set; }
        public decimal? price10sessions { get; set; }
        public string currencynm { get; set; }
        public string agreedconditions { get; set; }
        public string note { get; set; }
        public long? photodocumentid { get; set; }
        public virtual doc_document photodocument { get; set; }
        public virtual ICollection<pcm_calevent> pcm_calevent { get; set; }
        public virtual ICollection<pcm_order> pcm_order { get; set; }
    }
}
