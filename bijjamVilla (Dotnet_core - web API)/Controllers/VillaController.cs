
using AutoMapper;

using bijjamVilla__Dotnet_core___web_API_.Data;
using bijjamVilla__Dotnet_core___web_API_.DTO;
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
        private readonly IMapper _mapper;
        public VillaController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<villa>>> GetVillas()
        {
            return Ok(await _db.villa.ToListAsync());
        }

        [HttpGet("{id:Int}")]
        public async Task<ActionResult<villa>> GetVillas(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Villa ID must be greater then 0");
                }

                var villa = await _db.villa.FirstOrDefaultAsync(v => v.Id == id);
                if (villa == null)
                {
                    return NotFound($"Villa with ID {id} Was not Found");
                }

                return Ok(villa);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured while retriving villa with ID {id}: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<villa>> CreateVillas(villaCreateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest("Villa data is required");
                }
                villa Villa = _mapper.Map<villa>(villaDTO);
                    
                 await _db.villa.AddAsync(Villa);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateVillas), new {id =Villa.Id},Villa);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured while creating the  villa : {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<villa>> UpdateVillas(int id , villaUpdateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest("Villa data is required");
                }

                if (id != villaDTO.Id)
                {
                    return BadRequest("Villa ID in url dose not match Villa ID in request body");
                }

                var existingVilla = await _db.villa.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVilla == null)
                {
                    return NotFound($"Villa with ID {id} Was not Found");
                }


               _mapper.Map(villaDTO, existingVilla);
                existingVilla.UpdatedDate = DateTime.Now;
                await _db.SaveChangesAsync();
                return Ok(villaDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured while Updating the  villa : {ex.Message}");
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<villa>> DeleteVillas(int id)
        {
            try
            { 
                var existingVilla = await _db.villa.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVilla == null)
                {
                    return NotFound($"Villa with ID {id} Was not Found");
                }
                
                _db.villa.Remove(existingVilla);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured while Deleting the  villa : {ex.Message}");
            }
        }
    }
    

}
