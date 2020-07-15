using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using API_receive_LoraCat.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace API_receive_LoraCat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiveController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private CatLoraPostContext db;
        // GET: api/Receive
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST: api/Receive
        [HttpPost]
        public void Post([FromBody] ClassDATA value)
        {
            this.Log(value);
            dynamic Pay = DecodePayload(value.DevEUI_uplink.payload_hex);
            
              
            switch (value.DevEUI_uplink.FPort)
            {
                case "1": // command
                    DateTime dt = DateTime.Now;
                    string hexValue = ((long)((dt - new DateTime(1970, 1, 1)).TotalMilliseconds)).ToString("X");

                    if (hexValue.Length % 2 != 0)
                        hexValue = "0" + hexValue;

                    Uplink(value.DevEUI_uplink.DevEUI, hexValue, value.DevEUI_uplink.FPort);
                    break;
                case "2": // event
                    foreach (KeyValuePair<string, object> kvp in ((IDictionary<string, object>)Pay))
                    {
                        InsertDataEvent(value, kvp.Key, float.Parse(kvp.Value.ToString()));
                    }
                    //Line_Send("");
                    break;
                case "3": // data
                    foreach (KeyValuePair<string, object> kvp in ((IDictionary<string, object>)Pay))
                    {
                        InsertData(value, kvp.Key, float.Parse(kvp.Value.ToString()));
                        

                    }

                    break;
            }
        }

        private object DecodePayload(string payload_hex)
        {
            IDictionary<string, dynamic> obj = new ExpandoObject();

            var hex = StringToByteArray(payload_hex);

            

            int j = 1;
            for (int i = 1; i <= hex[0]; i++)
            {
                //get data type
                byte dataType = hex[j++];

                switch (dataType)
                {
                    case 0x00: //Normally
                        obj["Sensor" + i] = (byte)hex[j++];
                        break;
                    case 0x01: //Signed Char
                        obj["Sensor" + i] = (byte)hex[j++];
                        break;
                    case 0x02: //Unsigned Char
                        obj["Sensor" + i] = (byte)hex[j++];
                        break;
                    case 0x03: //Signed Short
                        obj["Sensor" + i] = ((short)hex[j] << 8) + hex[j + 1];
                        j += 2;
                        break;
                    case 0x04: //Unsigned Short
                        obj["Sensor" + i] = ((ushort)hex[j] << 8) + hex[j + 1];
                        j += 2;
                        break;
                    case 0x05: //Signed Long
                        obj["Sensor" + i] = ((int)hex[j] << 24) + ((int)hex[j + 1] << 16) + ((int)hex[j + 2] << 8) + hex[j + 3];
                        j += 4;
                        break;
                    case 0x06: //Unsigned Long
                        obj["Sensor" + i] = ((uint)hex[j] << 24) + ((uint)hex[j + 1] << 16) + ((uint)hex[j + 2] << 8) + hex[j + 3];
                        j += 4;
                        break;
                }
            }

            return obj ;
        }
        public byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        private void Uplink(string devEUI, string payloadhex, string targetpost)
        {
            string acctoken = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://loraiot.cattelecom.com/portal/iotapi/auth/token");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"username\":\"expert\"," +
                              "\"password\":\"ex159357\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var js = JsonConvert.DeserializeObject<Respon>(result);
                acctoken = js.access_token.Trim();

            }

            string urli = @"https://loraiot.cattelecom.com/portal/iotapi/core/devices/" + devEUI + "/downlinkMessages";

            var httpsend = (HttpWebRequest)WebRequest.Create(urli);
            httpsend.ContentType = "application/json";
            httpsend.Headers["Authorization"] = "Bearer " + acctoken;
            httpsend.Method = "POST";

            using (var streamWritersend = new StreamWriter(httpsend.GetRequestStream()))
            {
                string jsonsend = "{\"payloadHex\":\"" + payloadhex + "\"," +
                              "\"targetPorts\":\"" + targetpost + "\"}";

                streamWritersend.Write(jsonsend);
            }

            var httpResponsesend = (HttpWebResponse)httpsend.GetResponse();
            using (var streamReadersend = new StreamReader(httpResponsesend.GetResponseStream()))
            {
                var results = streamReadersend.ReadToEnd();
                var js = JsonConvert.DeserializeObject<ResponseUplink>(results);

                string logreturn = "Send Uplink " + js.status + "/" + js.payloadHex + "/" + js.targetPorts;
                this.Log(logreturn);
            }
        }

        private void InsertData(ClassDATA a, string b, float c)
        {
            using (db = new CatLoraPostContext())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var query = new Collecteddata()
                        {

                            //Timestamp = a.DevEUI_uplink.Time,
                            Timestamp = DateTime.Now,
                            DeviceAddress = a.DevEUI_uplink.DevAddr,
                            DevEui = a.DevEUI_uplink.DevEUI,
                            SensorId = b,
                            ValueSensor = c,
                            Fport = float.Parse(a.DevEUI_uplink.FPort.Replace(" ", "")),
                            FcntUp = float.Parse(a.DevEUI_uplink.FCntUp.Replace(" ", "")),
                            FcntDw = float.Parse(a.DevEUI_uplink.FCntDn.Replace(" ", "")),
                            Rssi = float.Parse(a.DevEUI_uplink.LrrRSSI.Replace(" ", "")),
                            Snr = float.Parse(a.DevEUI_uplink.LrrSNR.Replace(" ", "")),
                            SubBand = a.DevEUI_uplink.SubBand,
                            Channel = a.DevEUI_uplink.Channel,
                            Lrr = a.DevEUI_uplink.Lrrid,
                            Timereceive = DateTime.Now
                            // Ack = a.DevEUI_uplink.Time,
                            //DeliveryStatus = a.DevEUI_uplink.Time,
                            //Rx1 = a.DevEUI_uplink.Time,
                            //Rx2 = a.DevEUI_uplink.Time,
                            // Beacon = a.DevEUI_uplink.ba
                        };
                        db.Collecteddata.Add(query);
                        db.SaveChanges();
                        tran.Commit();
                        this.Log("Insert Data Commit");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        this.Log(ex.Message);
                    }
                }

            }

        }

        private void InsertDataEvent(ClassDATA a, string b, float c)
        {
            using (db = new CatLoraPostContext())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var query = new Collecteddata()
                        {

                            //Timestamp = a.DevEUI_uplink.Time,
                            Timestamp = DateTime.Now,
                            DeviceAddress = a.DevEUI_uplink.DevAddr,
                            DevEui = a.DevEUI_uplink.DevEUI,
                            SensorId = b,
                            ValueSensor = c,
                            Fport = float.Parse(a.DevEUI_uplink.FPort.Replace(" ", "")),
                            FcntUp = float.Parse(a.DevEUI_uplink.FCntUp.Replace(" ", "")),
                            FcntDw = float.Parse(a.DevEUI_uplink.FCntDn.Replace(" ", "")),
                            Rssi = float.Parse(a.DevEUI_uplink.LrrRSSI.Replace(" ", "")),
                            Snr = float.Parse(a.DevEUI_uplink.LrrSNR.Replace(" ", "")),
                            SubBand = a.DevEUI_uplink.SubBand,
                            Channel = a.DevEUI_uplink.Channel,
                            Lrr = a.DevEUI_uplink.Lrrid,
                            Timereceive = DateTime.Now
                            // Ack = a.DevEUI_uplink.Time,
                            //DeliveryStatus = a.DevEUI_uplink.Time,
                            //Rx1 = a.DevEUI_uplink.Time,
                            //Rx2 = a.DevEUI_uplink.Time,
                            // Beacon = a.DevEUI_uplink.ba
                        };
                        db.Collecteddata.Add(query);
                        db.SaveChanges();
                        tran.Commit();
                        this.Log("Insert Data Commit");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        this.Log(ex.Message);
                    }
                }

            }

        }

        public async void Line_Send(ClassDATA a,string info)
        {


            //string al = null;
            //if (info.Status == "0")
            //{
            //    al = "ระดับน้ำไกลผิวดิน";
            //}
            //else if (info.Status == "1")
            //{
            //    al = "ระดับน้ำไกลผิวดินมาก";
            //}
            //else if (info.Status == "2")
            //{
            //    al = "ระดับน้ำใกล้ผิวดินมาก";
            //}
            //else if (info.Status == "3")
            //{
            //    al = "ระดับน้ำใกล้ผิวดินมาก";
            //}
            //string mes = "รหัสบ่อ " + info.StationID + " ระดับน้ำ " + info.TagValue + " เวลา " + info.TimeStamp + " แจ้งเตือน " + al
            string mes = "123";
            var values = new Dictionary<string, string>
            {
               { "message", mes }

            };

            var content = new FormUrlEncodedContent(values);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));//ACCEPT header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "NZ5cLxBIYUQFlILxlOsNyr9elSsRmblqPg5arAZRZ6R");
            var response = await client.PostAsync("https://notify-api.line.me/api/notify", content);

            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
