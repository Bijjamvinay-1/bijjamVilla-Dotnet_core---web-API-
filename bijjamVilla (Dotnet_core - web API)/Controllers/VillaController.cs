
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
        public async Task<ActionResult<IEnumerable<villaDTO>>> GetVillas()
        {
            var villas = await _db.villa.ToListAsync();
            return Ok(_mapper.Map<List<villaDTO>>(villas));
        }

        [HttpGet("{id:Int}")]
        public async Task<ActionResult<villaDTO>> GetVillas(int id)
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

                return Ok(_mapper.Map<villaDTO>(villa));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured while retriving villa with ID {id}: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<villaDTO>> CreateVillas(villaCreateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest("Villa data is required");
                }

                var DuplicateVilla = await _db.villa.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());

                if (DuplicateVilla != null)
                {
                    return Conflict($"Villa with name {villaDTO.Name} already exists");
                }

                villa Villa = _mapper.Map<villa>(villaDTO);
                    
                 await _db.villa.AddAsync(Villa);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateVillas), new {id =Villa.Id},_mapper.Map<villaDTO>(Villa));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured while creating the  villa : {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<villaDTO>> UpdateVillas(int id , villaUpdateDTO villaDTO)
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

                var DuplicateVilla = await _db .villa.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower() && u.Id != id);

                if (DuplicateVilla != null)
                {
                    return Conflict($"Villa with name {villaDTO.Name} already exists");
                }

                _mapper.Map(villaDTO, existingVilla);
                existingVilla.UpdatedDate = DateTime.Now;
                await _db.SaveChangesAsync();
                return Ok(_mapper.Map<villaDTO>(existingVilla));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured while Updating the  villa : {ex.Message}");
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<villaDTO>> DeleteVillas(int id)
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
                return Ok(_mapper.Map<villaDTO>(existingVilla));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured while Deleting the  villa : {ex.Message}");
            }
        }
    }
    

}
