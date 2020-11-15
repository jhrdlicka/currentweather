using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class pcm_gcalevent
    {
        public long id { get; set; }
        public string gcalid { get; set; }
        public long? customerid { get; set; }
        public long? caleventid { get; set; }
        public string gcaljson { get; set; }
    }
}
