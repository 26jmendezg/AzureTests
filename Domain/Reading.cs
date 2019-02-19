using System;

namespace Domain
{
    public class Reading
    {
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime ReadingTime { get; set; }
        public int Value { get; set; }
    }
}