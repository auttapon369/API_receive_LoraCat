using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_receive_LoraCat
{
    public class ClassDATA
    {
        public DevEUIUplink DevEUI_uplink { get; set; }
    }
    
    public class ValuePay
    {
        public string sensor_name { get; set; }
        public float value_sensor { get; set; }
    }
    public class Respon
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
    }
    public class ResponseUplink
    {
        public string payloadHex { get; set; }
        public string targetPorts { get; set; }
        public string status { get; set; }

    }

    public class Lrr
    {
        public string Lrrid { get; set; }
        public string Chain { get; set; }
        public string LrrRSSI { get; set; }
        public string LrrSNR { get; set; }
        public string LrrESP { get; set; }
    }

    public class Lrrs
    {
        public List<Lrr> Lrr { get; set; }
    }

    public class Alr
    {
        public string pro { get; set; }
        public string ver { get; set; }
    }

    public class CustomerData
    {
        public Alr alr { get; set; }
    }

    public class DevEUIUplink
    {
        public DateTime Time { get; set; }
        public string DevEUI { get; set; }
        public string DevAddr { get; set; }
        public string FPort { get; set; }
        public string FCntUp { get; set; }
        public string ADRbit { get; set; }
        public string MType { get; set; }
        public string FCntDn { get; set; }
        public string payload_hex { get; set; }
        public string mic_hex { get; set; }
        public string Lrcid { get; set; }
        public string LrrRSSI { get; set; }
        public string LrrSNR { get; set; }
        public string SpFact { get; set; }
        public string SubBand { get; set; }
        public string Channel { get; set; }
        public string DevLrrCnt { get; set; }
        public string Lrrid { get; set; }
        public string Late { get; set; }
        public string LrrLAT { get; set; }
        public string LrrLON { get; set; }
        public Lrrs Lrrs { get; set; }
        public string CustomerID { get; set; }
        public CustomerData CustomerData { get; set; }
        public string ModelCfg { get; set; }
        public string InstantPER { get; set; }
        public string MeanPER { get; set; }
    }

}
