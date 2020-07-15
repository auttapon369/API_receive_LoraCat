using System;
using System.Collections.Generic;

namespace API_receive_LoraCat.Models
{
    public partial class Collecteddata
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string DeviceAddress { get; set; }
        public string DevEui { get; set; }
        public string SensorId { get; set; }
        public float ValueSensor { get; set; }
        public float Fport { get; set; }
        public float FcntUp { get; set; }
        public float FcntDw { get; set; }
        public float Rssi { get; set; }
        public float Snr { get; set; }
        public string SubBand { get; set; }
        public string Channel { get; set; }
        public string Lrr { get; set; }
        public string Ack { get; set; }
        public string DeliveryStatus { get; set; }
        public string Rx1 { get; set; }
        public string Rx2 { get; set; }
        public string Beacon { get; set; }
        public DateTime Timereceive { get; set; }
    }
}
