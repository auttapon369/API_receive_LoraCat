using System;
using System.Collections.Generic;

namespace API_receive_LoraCat.Models
{
    public partial class Station
    {
        public int Id { get; set; }
        public string DeviceAddress { get; set; }
        public string DevEui { get; set; }
        public string NameStation { get; set; }
        public string AddressStation { get; set; }
        public float? Lat { get; set; }
        public float? Lng { get; set; }
        public int Status { get; set; }
    }
}
