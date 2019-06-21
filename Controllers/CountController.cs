using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest;

namespace K8sController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountController : ControllerBase
    {
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id, [FromQuery(Name = "namespace")] string ns,
            [FromQuery(Name = "name")] string name)
        {
            try
            {
                var client = Config.Instance.GetCli();
                var scale = await client.ReadNamespacedDeploymentScaleWithHttpMessagesAsync(name, ns);
                return Ok(scale.Body.Spec.Replicas);
            }
            catch (HttpOperationException e)
            {
                return NotFound(e.Message);
            }
            catch (MissingMemberException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            
        }
    }
}