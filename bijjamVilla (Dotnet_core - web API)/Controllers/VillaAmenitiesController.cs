using AutoMapper;

using bijjamVilla__Dotnet_core___web_API_.Data;
using BijjamVillaDTO;
using bijjamVilla__Dotnet_core___web_API_.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bijjamVilla__Dotnet_core___web_API_.Controllers
{
    [Route("api/VillaAmenties")]
    [ApiController]
    public class VillaAmentiesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaAmentiesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        [HttpGet]
        //[Authorize(Roles ="Admin")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmenitiesDTO>>>> GetVillaAmenities()
        {
            var villas = await _db.VillaAmenities.ToListAsync();
            var dtoResponseVillaAmenities = _mapper.Map<List<VillaAmenitiesDTO>>(villas);
            var response = ApiResponse<IEnumerable<VillaAmenitiesDTO>>.OK(dtoResponseVillaAmenities, "Villa Amenities retrieved successfully");
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        //[AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> GetVillaAmenitiesById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("VillaAmenities ID must be greater than 0"));
                }

                var villaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(u => u.Id == id);
                if (villaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"VillaAmenities with ID {id} was not found"));
                }
                return Ok(ApiResponse<VillaAmenitiesDTO>.OK(_mapper.Map<VillaAmenitiesDTO>(villaAmenities), "Records retrieved successfully"));
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while creating the villa:", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> CreateVillaAmenities(VillaAmenitiesCreateDTO VillaAmenitiesDTO)
        {
            try
            {
                if (VillaAmenitiesDTO == null)
                {
                    return BadRequest(ApiResponse<object>.Badrequest("Villa Amenities data is required"));
                }


                var villaExists = await _db.villa.FirstOrDefaultAsync(u => u.Id == VillaAmenitiesDTO.VillaId);

                if (villaExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Villa with the ID '{VillaAmenitiesDTO.VillaId}'does not exist."));
                }

                VillaAmenities villaAmenities = _mapper.Map<VillaAmenities>(VillaAmenitiesDTO);
                villaAmenities.CreatedDate = DateTime.Now;
                await _db.VillaAmenities.AddAsync(villaAmenities);
                await _db.SaveChangesAsync();

                var response = ApiResponse<VillaAmenitiesDTO>.CreatedAt(_mapper.Map<VillaAmenitiesDTO>(villaAmenities), "Villa Amenities created successfully");
                return CreatedAtAction(nameof(CreateVillaAmenities), new { id = villaAmenities.Id }, response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while creating the villa amenities:", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> UpdateVillaAmenities(int id, VillaAmenitiesUpdateDTO VillaAmenitiesDTO)
        {
            try
            {
                if (VillaAmenitiesDTO == null)
                {
                    return BadRequest(ApiResponse<object>.Badrequest("Villa Amenities data is required"));
                }

                if (id != VillaAmenitiesDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.Badrequest("Villa Amenities ID in URL does not match VillaAmenities ID in request body"));
                }

                var villaExists = await _db.villa.FirstOrDefaultAsync(u => u.Id == VillaAmenitiesDTO.VillaId);

                if (villaExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Villa with the ID '{VillaAmenitiesDTO.VillaId}'does not exist."));
                }

                var existingVillaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVillaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa Amenities with ID {id} was not found"));
                }



                _mapper.Map(VillaAmenitiesDTO, existingVillaAmenities);
                existingVillaAmenities.UpdatedDate = DateTime.Now;

                await _db.SaveChangesAsync();
                var response = ApiResponse<VillaAmenitiesDTO>.OK(_mapper.Map<VillaAmenitiesDTO>(existingVillaAmenities), "Villa Amenities updated successfully");
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while creating the villa amenities:", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVillaAmenities(int id)
        {
            try
            {
                var existingVillaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVillaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa Amenities with ID {id} was not found"));
                }

                _db.VillaAmenities.Remove(existingVillaAmenities);
                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.Nocontent("Villa Amenities deleted successfully");
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while creating the villa amenities:", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

    }
}
