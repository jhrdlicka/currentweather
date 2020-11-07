﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace currentweather.Models
{
    public partial class Pcm_OrderSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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