using AutoMapper;

using bijjamVilla__Dotnet_core___web_API_.Data;
using bijjamVilla__Dotnet_core___web_API_.DTO;
using bijjamVilla__Dotnet_core___web_API_.Services;

using Microsoft.AspNetCore.Mvc;

namespace bijjamVilla__Dotnet_core___web_API_.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;

        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Register([FromBody] RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                //auth service
                if (registerationRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.Badrequest("Registration data is required"));
                }
                if (await _authService.IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    return Conflict(ApiResponse<object>.Conflict($"Email already exists '{registerationRequestDTO.Email}' already exists"));
                }
                var user = await _authService.RegisterAsync(registerationRequestDTO);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, "An error occurred while creating the user"));
                }
                var response = ApiResponse<UserDTO>.CreatedAt(user, "User registered successfully");
                // Return 201 Created. If you have a GET action for the created user, prefer CreatedAtAction pointing to that action.
                return Created(string.Empty, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, "An error occurred while processing the request"));
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LoginResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                //auth service
                if (loginRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.Badrequest("Login data is required"));
                }

                var loginResponse = await _authService.LoginAsync(loginRequestDTO);

                if (loginResponse == null)
                {
                    return BadRequest(ApiResponse<object>.Badrequest("Login Failed may be Invalid email or password "));
                }

                var response = ApiResponse<LoginResponseDTO>.OK(loginResponse, "User logged in successfully");

               return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while processing the request {ex.Message}") );
            }
        }

    }
}
