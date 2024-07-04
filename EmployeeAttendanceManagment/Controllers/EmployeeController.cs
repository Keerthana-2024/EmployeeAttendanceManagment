using EmployeeAttendanceManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeManagementDbContext _context;

        public EmployeesController(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        // POST: api/employees/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterEmployee([FromBody] Employee employee)
        {
            try
            {
                // Validate the model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the username is already taken
                if (await _context.Employees.AnyAsync(e => e.Username == employee.Username))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return BadRequest(ModelState);
                }

                // Hash the password before saving
                employee.PasswordHash = HashPassword(employee.PasswordHash); // Implement your hashing method

                // Set timestamps
                employee.CreatedAt = DateTime.UtcNow;
                employee.UpdatedAt = DateTime.UtcNow;

                // Add and save the employee
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Employee registered successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // POST: api/employees/signin
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] EmployeeLogin login)
        {
            try
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Username == login.Username);

                if (employee == null || !VerifyPassword(login.Password, employee.PasswordHash))
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                return Ok(new { message = "Sign-in successful", employeeId = employee.EmployeeID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // Example of a password hashing method
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // Example of password verification
        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInputPassword = HashPassword(password);
            return hashedInputPassword == hashedPassword;
        }
    }

    
}
