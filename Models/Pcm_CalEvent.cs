using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace currentweather.Models
{
    public partial class Pcm_CalEvent
    {
        public Pcm_CalEvent()
        {
            OrderSessions = new HashSet<Pcm_OrderSession>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime? Start { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Title { get; set; }
        public long? CustomerId { get; set; }
        public string GcalId { get; set; }
        public string GcalStatus { get; set; }
        public string GcalHtmllink { get; set; }
        public DateTime? GcalCreated { get; set; }
        public DateTime? GcalUpdated { get; set; }
        public string GcalSummary { get; set; }
        public string GcalDescription { get; set; }
        public string GcalLocation { get; set; }
        public DateTime? GcalStart { get; set; }
        public DateTime? GcalEnd { get; set; }
        public string GcalJson { get; set; }
        public string XOrdered { get; set; }
        public string Invoicetext { get; set; }
        public decimal? Price { get; set; }
        public string CurrencyNm { get; set; }
        public DateTime? Canceled { get; set; }
        public string CanceledReason { get; set; }

        public virtual Pcm_Customer Customer { get; set; }
        public virtual ICollection<Pcm_OrderSession> OrderSessions { get; set; }
    }
}
