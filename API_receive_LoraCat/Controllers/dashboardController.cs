using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_receive_LoraCat.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]

    public class dashboardController : ControllerBase
    {
        // GET: api/map

        [EnableCors("AnotherPolicy")]
        [HttpGet]
        public dynamic Get()
        {
           

            Properties b = new Properties();
            b.state = "Expert Engineering";
            b.name = "Expert Engineering";
            b.sumletter = 5;
            b.lastupdate = DateTime.Now;

            var tt = new List<double>() { 100.6508802, 13.6900943 };
            Geometry c = new Geometry();
            c.type = "Point";
            c.coordinates = tt;
            //c.coordinates.Add(32.361538);

            List<Feature> d = new List<Feature>();

            Feature e = new Feature();
            e.type = "Point";
            e.geometry = c;
            e.properties = b;

            d.Add(e);

            Cgeojson a = new Cgeojson();
            a.type = "FeatureCollection";
            a.features = d;
            




            return a;


        }

        // GET: api/map/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}


    }
}
