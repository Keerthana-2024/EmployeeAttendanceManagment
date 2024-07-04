using EmployeeAttendanceManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace EmployeeAttendanceManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationChartController : ControllerBase
    {
        private readonly EmployeeManagementDbContext _context;

        public OrganizationChartController(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        // GET: api/organizationchart
        [HttpGet]
        public async Task<IActionResult> GetOrganizationChart()
        {
            var organizationChart = await _context.OrganizationCharts
                .Include(oc => oc.Employee)
                .Include(oc => oc.Manager)
                .ToListAsync();

            return Ok(organizationChart);
        }

        // POST: api/organizationchart
        [HttpPost]
        public async Task<IActionResult> CreateOrganizationChart([FromBody] OrganizationChart organizationChart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.OrganizationCharts.Add(organizationChart);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Organization chart entry created successfully" });
        }
    }

}
