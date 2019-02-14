using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Device
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Threshold Threshold { get; set; }
        public IEnumerable<Reading> Readings { get; set; }
    }
}
