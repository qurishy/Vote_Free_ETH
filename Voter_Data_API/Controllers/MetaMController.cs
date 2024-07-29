using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voter_Data_API.MODELS;

namespace Voter_Data_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaMController : ControllerBase
    {

        [HttpPost("verify")]
        public IActionResult Verify([FromBody] MetaMaskData data)
        {
            return Ok();
        }
    }
}
