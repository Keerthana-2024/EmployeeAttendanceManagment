using EmployeeAttendanceManagment.Model;
using EmployeeAttendanceManagment.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAttendanceManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public IActionResult GenerateToken([FromBody] TokenRequest model)
        {
            // Validate user credentials here if needed
            // For simplicity, assuming validation is done elsewhere

            var token = _tokenService.GenerateToken(model.Username);
            return Ok(new { Token = token });
        }
    }
}
