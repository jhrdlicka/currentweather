﻿using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class doc_document
    {
        public doc_document()
        {
            pcm_invoicescandocument = new HashSet<pcm_invoice>();
            pcm_invoicesourcedocument = new HashSet<pcm_invoice>();
            pcm_customer = new HashSet<pcm_customer>();
        }

        public long id { get; set; }
        public byte[] content { get; set; }
        public string url { get; set; }

        public virtual ICollection<pcm_invoice> pcm_invoicescandocument { get; set; }
        public virtual ICollection<pcm_invoice> pcm_invoicesourcedocument { get; set; }
        public virtual ICollection<pcm_customer> pcm_customer { get; set; }
    }
}
