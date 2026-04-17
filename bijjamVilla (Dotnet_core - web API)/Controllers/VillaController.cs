
using bijjamVilla__Dotnet_core___web_API_.Data;
using bijjamVilla__Dotnet_core___web_API_.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bijjamVilla__Dotnet_core___web_API_.Controllers
{
    [Route("api/villas")]
    [ApiController]
    public class VillaController : ControllerBase      
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<villa>>> GetVillas()                      
        {
            return Ok (await _db.villa.ToListAsync());
        }

        [HttpGet("{id:Int}")]    
        public string GetVillas(int id)
        {
            return "Get Villa with id: " + id;
        }
    }
}
