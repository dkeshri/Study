using Microsoft.AspNetCore.Mvc;

namespace Dkeshri.WebApi.Controllers.Base
{
    [ApiController]
    [Route("[controller]")]
    public abstract class WebApiControllerBase : ControllerBase
    {
    }
}
