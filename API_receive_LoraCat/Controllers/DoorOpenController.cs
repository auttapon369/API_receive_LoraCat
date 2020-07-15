using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_receive_LoraCat.Models;
using Microsoft.AspNetCore.Cors;

namespace API_receive_LoraCat.Controllers
{
    [Route("report/[controller]")]
    [ApiController]
    public class DoorOpenController : ControllerBase
    {
        public CatLoraPostContext db;
        public DoorOpenController(CatLoraPostContext _db)
        {
            db = _db;
        }
        // GET: api/DoorOpen
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [EnableCors("AnotherPolicy")]
        //GET: api/letter/5
        [HttpGet("{dt1}/{dt2}")]
        public dynamic Get(string dt1,string dt2)
        {
           
                DateTime startDt = (dt1 == "1970-01-01") ? DateTime.Now.Date : Convert.ToDateTime(dt1);
                DateTime endDt = (dt2 == "1970-01-01") ? DateTime.Now.Date : Convert.ToDateTime(dt2);
                var query = (from a in db.Collecteddataevent
                             group a by a.DeviceAddress into grData
                             join b in db.Station on grData.First().DevEui equals b.DevEui
                             where grData.First().Timestamp.Date >= startDt.Date
                             && grData.First().Timestamp.Date <= endDt.Date    
                             orderby grData.First().Timestamp descending 
                             select new
                             {
                                 grData.First().Timestamp,
                                 b.DevEui,
                                 b.DeviceAddress,
                                 b.NameStation,
                                 grData.First().SensorId,
                                 grData.First().ValueSensor

                             }).ToList();
                return query;
            

        }


    }
}
