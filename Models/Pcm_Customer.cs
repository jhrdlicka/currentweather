using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace currentweather.Models
{
    [DataContract]
    public partial class pcm_customer
    {
        public pcm_customer()
        {
            pcm_calevent = new HashSet<pcm_calevent>();
            pcm_order = new HashSet<pcm_order>();
        }
        [DataMember]
        public long id { get; set; }
        [DataMember]
        public string firstname { get; set; }
        [DataMember]
        public string surname { get; set; }
        [DataMember]
        public string emailcalendar { get; set; }
        [DataMember]
        public string emailinvoice { get; set; }
        [DataMember]
        public string phone { get; set; }
        [JsonIgnore]
        public byte[] photo { get; set; }
        [DataMember]
        public bool? active { get; set; }
        [DataMember]
        public decimal? pricesession { get; set; }
        [DataMember]
        public decimal? price10sessions { get; set; }
        [DataMember]
        public string currencynm { get; set; }
        [DataMember]
        public string agreedconditions { get; set; }
        [DataMember]
        public string note { get; set; }
        [DataMember]
        public virtual ICollection<pcm_calevent> pcm_calevent { get; set; }
        [DataMember]
        public virtual ICollection<pcm_order> pcm_order { get; set; }
    }

}
