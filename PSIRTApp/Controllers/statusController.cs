using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSIRTApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class statusController : ControllerBase
    {


        [HttpGet]
        [Route("GetStatus")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetStatus()
        {
            var result = new HttpResponseMessage();
            result.StatusCode = System.Net.HttpStatusCode.OK;

            return result;
        }
    }
}
