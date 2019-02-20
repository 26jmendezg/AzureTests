using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Domain
{
    public class Device
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public Threshold Threshold { get; set; }
        public ICollection<Reading> Readings { get; set; }
    }
}
