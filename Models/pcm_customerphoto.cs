using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace currentweather.Models
{
    public partial class pcm_customerphoto
    {
        public pcm_customerphoto()
        {
        }
        public long id { get; set; }
        public byte[] photo { get; set; }
        public virtual pcm_customer customer { get; set; }
    }

}
