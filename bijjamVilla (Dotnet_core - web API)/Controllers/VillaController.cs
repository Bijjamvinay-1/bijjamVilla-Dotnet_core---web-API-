
using Microsoft.AspNetCore.Mvc;

namespace bijjamVilla__Dotnet_core___web_API_.Controllers
{
    [Route("api/villas")]
    [ApiController]
    public class VillaController : ControllerBase      //if it is a MVC then we can use only Controller instead   of ControllerBase.
    {
        [HttpGet]
        public string GetVillas()
        {
            return "Get Villa";
        }

        [HttpGet("{id:Int}")]    // Route parameter Binding
        public string GetVillas(int id)
        {
            return "Get Villa with id: " + id;
        }
    }
}
