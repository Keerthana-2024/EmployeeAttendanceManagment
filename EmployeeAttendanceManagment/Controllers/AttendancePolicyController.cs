using EmployeeAttendanceManagement.Model;
using EmployeeAttendanceManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace EmployeeAttendanceManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly EmployeeManagementDbContext _context;

        public AttendanceController(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        // GET: api/attendancepolicy
        [HttpGet]
        public async Task<IActionResult> GetAttendancePolicies()
        {
            var policies = await _context.AttendancePolicies.ToListAsync();
            return Ok(policies);
        }

        // POST: api/attendancepolicy
        [HttpPost]
        public async Task<IActionResult> CreateAttendancePolicy([FromBody] AttendancePolicy policy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AttendancePolicies.Add(policy);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Attendance policy created successfully" });
        }
        [HttpPost("mark")]
        public async Task<IActionResult> MarkAttendance(MarkAttendanceRequest model)
        {
            try
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeID == model.EmployeeID);

                if (employee == null)
                {
                    return NotFound("Employee not found.");
                }

                var policy = await _context.AttendancePolicies
                    .FirstOrDefaultAsync(p => p.PolicyID == employee.AttendancePolicyID);

                if (policy == null)
                {
                    return NotFound("Attendance policy not found.");
                }

                var allowedLocations = policy.AllowedLocations.Split(',').Select(loc => loc.Trim());
                if (!allowedLocations.Contains(model.CurrentLocation))
                {
                    return BadRequest("Location not allowed by the policy.");
                }

                var attendance = new Attendance
                {
                    EmployeeID = model.EmployeeID,
                    Date = DateTime.UtcNow, // or use model.Date if provided
                    Location = model.CurrentLocation,
                    IsPresent =true
                };

                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Attendance marked successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception
                // For example: _logger.LogError(ex, "Error marking attendance.");
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}