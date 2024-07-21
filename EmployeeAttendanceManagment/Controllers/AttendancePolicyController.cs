using EmployeeAttendanceManagement.Model;
using EmployeeAttendanceManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace EmployeeAttendanceManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancePolicyController : ControllerBase
    {
        private readonly EmployeeManagementDbContext _context;

        public AttendancePolicyController(EmployeeManagementDbContext context)
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
    }

}
