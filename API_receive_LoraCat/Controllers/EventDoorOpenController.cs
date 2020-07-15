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
    public class EventDoorOpenController : ControllerBase
    {
        public CatLoraPostContext db;
        public EventDoorOpenController(CatLoraPostContext _db)
        {
            db = _db;
        }

        [EnableCors("AnotherPolicy")]
        //GET: api/letter/5
        [HttpGet]
        public dynamic Get()
        {
           
                // DateTime startDt = (dt1 == "1970-01-01") ? DateTime.Now.Date : Convert.ToDateTime(dt1);
                //DateTime endDt = (dt2 == "1970-01-01") ? DateTime.Now.Date : Convert.ToDateTime(dt2);
                //var re = (from a in db.Station
                //          join b in db.Collecteddataevent on a.DevEui equals b.DevEui into yg
                //          from y in yg.DefaultIfEmpty()
                //          select new
                //          {
                //              y.Timestamp,
                //              y.DevEui,
                //              y.DeviceAddress,
                //              a.NameStation,
                //              a.AddressStation,
                //              y.SensorId,
                //              y.ValueSensor
                //          }).ToList();
                var re = (from a in db.Station
                         select new
                         {
                             Timestamp = db.Collecteddataevent.Where(x => x.DevEui == a.DevEui).OrderByDescending(x => x.Id).Select(x => x.Timestamp).FirstOrDefault(),
                             DevEui = db.Collecteddataevent.Where(x => x.DevEui == a.DevEui).OrderByDescending(x => x.Id).Select(x => x.DevEui).FirstOrDefault(),
                             DeviceAddress = db.Collecteddataevent.Where(x => x.DevEui == a.DevEui).OrderByDescending(x => x.Id).Select(x => x.ValueSensor).FirstOrDefault(),
                             a.NameStation,
                             a.AddressStation,
                             SensorId = db.Collecteddataevent.Where(x => x.DevEui == a.DevEui).OrderByDescending(x => x.Id).Select(x => x.SensorId).FirstOrDefault(),
                             ValueSensor = db.Collecteddata.Where(x => x.DevEui == a.DevEui).OrderByDescending(x => x.Id).Select(x => x.ValueSensor).FirstOrDefault(),

                         }).ToList();


                var query = from a in re
                            where (a.Timestamp.Year <= DateTime.Now.Year)
                            && (a.Timestamp.Month <= DateTime.Now.Month)
                             && (a.Timestamp.Day < DateTime.Now.Day)
                            
                            select new
                            {
                                a.Timestamp,
                                a.DevEui,
                                a.DeviceAddress,
                                a.NameStation,
                                a.AddressStation,
                                a.SensorId,
                                a.ValueSensor 

                            };

                return query;
            }

        


    }
}
