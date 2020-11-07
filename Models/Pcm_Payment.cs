using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace currentweather.Models
{
    public partial class Pcm_Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyNm { get; set; }
        public DateTime? Date { get; set; }
        public string TypeNm { get; set; }
        public string ReferenceNr { get; set; }
        public string PaidBy { get; set; }
        public string Description { get; set; }
        public string AccountNr { get; set; }

        public virtual Pcm_Invoice Invoice { get; set; }
    }
}
