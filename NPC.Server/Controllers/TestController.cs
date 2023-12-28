using Microsoft.AspNetCore.Mvc;
using NPC.Server.Models;

namespace NPC.Server.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        [HttpGet]
        public async Task<GenericResponse> GetOverTypes()
        {
            GenericResponse resp = new GenericResponse();
            try
            {
                // resp.data = await _gameservice.GetOverTypes();
                resp.data = new { name = "sabarish" };
                resp.Status = 1;
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
            }
            return resp;
        }



    }
}
