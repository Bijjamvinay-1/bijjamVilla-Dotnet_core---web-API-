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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<villaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<villaDTO>>>> GetVillas()
        {
            var villas = await _db.villa.ToListAsync();
            var dtoResponseVilla = _mapper.Map<List<villaDTO>>(villas);
            var response = ApiResponse<IEnumerable<villaDTO>>.OK(dtoResponseVilla, "Villas retrieved successfully");
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<villaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<villaDTO>>> GetVillas(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<villaDTO>.NotFound("Villa Id must be a positive integer"));
                }

                var villa = await _db.villa.FirstOrDefaultAsync(v => v.Id == id);
                if (villa == null)
                {
                    return NotFound(ApiResponse<villaDTO>.NotFound($"Villa with ID {id} was not found"));
                }
                var dtoResponseVilla = _mapper.Map<villaDTO>(villa);
                return Ok(ApiResponse<villaDTO>.OK(dtoResponseVilla, $"Villa with ID {id} retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured while retriving villa with ID {id}: {ex.Message}");
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<villaDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<villaDTO>>> CreateVillas(villaCreateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest(ApiResponse<villaDTO>.Badrequest("Villa data is required"));
                }

                var DuplicateVilla = await _db.villa.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());

                if (DuplicateVilla != null)
                {
                    return Conflict(ApiResponse<villaDTO>.Conflict($"Villa with name {villaDTO.Name} already exists"));
                }

                villa Villa = _mapper.Map<villa>(villaDTO);
                    
                 await _db.villa.AddAsync(Villa);
                await _db.SaveChangesAsync();
                var response = ApiResponse<villaDTO>.CreatedAt(_mapper.Map<villaDTO>(Villa), "Villa created successfully");
                return CreatedAtAction(nameof(CreateVillas), new {id =Villa.Id}, response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"Error occured while creating the villa: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<villaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<villaDTO>> UpdateVillas(int id , villaUpdateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest(ApiResponse<villaDTO>.Badrequest("Villa  can't be null value"));
                }

                if (id != villaDTO.Id)
                {
                    return BadRequest(ApiResponse<villaDTO>.Badrequest("Villa ID in url dose not match Villa ID in request body"));
                }

                var existingVilla = await _db.villa.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVilla == null)
                {
                    return NotFound(ApiResponse<villaDTO>.NotFound($"Villa with ID {id} Was not Found"));
                }

                var DuplicateVilla = await _db .villa.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower() && u.Id != id);

                if (DuplicateVilla != null)
                {
                    return Conflict(ApiResponse<villaDTO>.Conflict($"Villa with name {villaDTO.Name} already exists"));
                }

                _mapper.Map(villaDTO, existingVilla);
                existingVilla.UpdatedDate = DateTime.Now;
                await _db.SaveChangesAsync();
                var dtoResponseVilla = _mapper.Map<villaDTO>(existingVilla);
                return Ok(ApiResponse<villaDTO>.OK(dtoResponseVilla, "Villa updated successfully"));
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"Error occured while Updating the villa: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<villaDTO>>> DeleteVillas(int id)
        {
            try
            { 
                var existingVilla = await _db.villa.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVilla == null)
                {
                    return NotFound(ApiResponse<villaDTO>.NotFound($"Villa with ID {id} Was not Found"));
                }
                
                _db.villa.Remove(existingVilla);
                await _db.SaveChangesAsync();
                var response = ApiResponse<villaDTO>.Nocontent($"Villa with ID {id} deleted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"Error occured while Deleting the  villa : {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }
    }
}
