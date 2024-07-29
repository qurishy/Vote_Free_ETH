using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Voter_Data_API.MODELS;
using Voter_Data_API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Voter_Data_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoterInfoController : ControllerBase

    {

        private readonly IVotersPersonelService _votersPersonInfo;

        public VoterInfoController(IVotersPersonelService votersPersonInfo)
        {
            _votersPersonInfo = votersPersonInfo;
            
        }


        // GET: api/VoterInfoController
        [HttpGet]
        public async Task<IActionResult>  GetAll()
        {
          var data =   await _votersPersonInfo.GetAllVotersAsync();

            return Ok(data);
        }

        // GET api/VoterInfoController/5
        [HttpGet("{id}")]
        public async Task<IActionResult>  Get(string id)
        {
            var data= await _votersPersonInfo.GetVoterAsync(id);

            if (data == null) {
                return NotFound();
            }

            return Ok(data);
        }

        // POST api/VoterInfoController
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VoterPersonelInfo Voty)
        {
            await _votersPersonInfo.CreateVoterAsync(Voty);

            return Ok("Created Successfully");

        }

        // PUT api/VoterInfoController/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] VoterPersonelInfo value)
        {
            var  valid = await _votersPersonInfo.GetVoterAsync(id);


            if (valid.Id == value.Id) {

                var data = _votersPersonInfo.UpdateVoterAsync(id,value);


                return Ok("updated Successfully");
            }

            return NotFound();

           

        }

        // DELETE api/VoterInfoController/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var valid = await _votersPersonInfo.GetVoterAsync(id);

            if(valid == null)
                return NotFound();
           
           await _votersPersonInfo.DeleteVoterAsync(id);

            return Ok("Deleted Successfully");
        }
    }
}
