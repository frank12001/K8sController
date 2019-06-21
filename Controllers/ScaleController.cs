using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest;

namespace K8sController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScaleController : ControllerBase
    {
        public class Msg
        {
            public string M1 = "need input QueryString namespace&name&count";
            public string M2 ="same replicas count";
            public string M3 ="can't find deployment";
            public string M4 = "Scale deploy replicas to count.";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id, [FromQuery(Name = "namespace")] string ns,
            [FromQuery(Name = "name")] string name, [FromQuery(Name = "replicas")] uint replicas)
        {
            try
            {
                var client = Config.Instance.GetCli();
                var scale = await client.ReadNamespacedDeploymentScaleWithHttpMessagesAsync(name, ns);
                if (scale.Body.Spec.Replicas == replicas)
                {
                    throw new MissingMemberException(ErrorMsg.Instance.Scale.M2);
                }
                scale.Body.Spec.Replicas = (int) replicas;
                await client.ReplaceNamespacedDeploymentScaleWithHttpMessagesAsync(scale.Body, scale.Body.Metadata.Name,
                    scale.Body.Metadata.NamespaceProperty);
            }
            catch (HttpOperationException e)
            {
                return NotFound(ErrorMsg.Instance.Scale.M3);
            }
            catch (MissingMemberException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            
            return Ok(ErrorMsg.Instance.Scale.M4.Replace("count",replicas.ToString()));
        }
    }
}