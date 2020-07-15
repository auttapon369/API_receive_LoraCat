using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_receive_LoraCat.Models;

namespace API_receive_LoraCat.Controllers
{
    [Route("report/[controller]")]
    [ApiController]
    public class letterController : ControllerBase
    {
        // GET: api/letter
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [EnableCors("AnotherPolicy")]
        //GET: api/letter/5
        [HttpGet("{dt1}/{dt2}")]
        public dynamic Get(string dt1, string dt2)
        {
            using (var db = new CatLoraPostContext())
            {
                DateTime startDt =(dt1=="1970-01-01")?DateTime.Now.Date:Convert.ToDateTime(dt1) ;
                DateTime endDt = (dt2 == "1970-01-01") ? DateTime.Now.Date : Convert.ToDateTime(dt2);
                var s1 = "Sensor1";
                var s2 = "Sensor2";
                var query = (from a in db.Collecteddata
                             join b in db.Station on a.DevEui equals b.DevEui
                             where a.Timestamp.Date >= startDt.Date 
                             && a.Timestamp.Date <= endDt.Date
                             orderby a.Timestamp descending
                             select new 
                            { 
                                a.Timestamp,
                                b.DevEui,
                                b.DeviceAddress,
                                b.NameStation,
                                a.SensorId,
                                a.ValueSensor

                            }).ToList();

                var re = from data in query
                         //where s1.Contains(data.SensorId.Trim()) && s2.Contains(data.SensorId.Trim())
                         group data by new { Timestamp = data.Timestamp.Date.AddHours(data.Timestamp.Hour) } into g
                        
                         orderby g.First().Timestamp descending
                          select new
                          {
                              g.First().Timestamp,
                              g.First().DevEui,
                              g.First().DeviceAddress,
                              g.First().NameStation,
                              g.First().SensorId,
                              val = g.Sum(x => x.ValueSensor)


                          };
                return re;
            }
            
        }


    }
}
