using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_receive_LoraCat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        // GET: api/test
        [HttpGet]
        public async Task<dynamic> Get()
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://cctvexpert.dyndns.org:8082/api_catLora/api/dashboard");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return new string[] { "value1", "value2" };
        }

        
    }
}
