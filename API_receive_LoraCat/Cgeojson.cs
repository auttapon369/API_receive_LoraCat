using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_receive_LoraCat.Controllers
{
    public class Cgeojson
    {
        public string type { get; set; }
        public IList<Feature> features { get; set; }
    }
    public class Geometry
    {
        public string type { get; set; }
        public IList<double> coordinates { get; set; }
    }

    public class Properties
    {
        public string state { get; set; }
        public string name { get; set; }
        public int sumletter { get; set; }
        public DateTime lastupdate { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

}
