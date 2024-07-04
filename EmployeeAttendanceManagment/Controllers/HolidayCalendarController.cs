using EmployeeAttendanceManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace EmployeeAttendanceManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayCalendarController : ControllerBase
    {
        private readonly EmployeeManagementDbContext _context;

        public HolidayCalendarController(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        // GET: api/holidaycalendar
        [HttpGet]
        public async Task<IActionResult> GetHolidayCalendar()
        {
            var holidayCalendar = await _context.HolidayCalendars.ToListAsync();
            return Ok(holidayCalendar);
        }

        // POST: api/holidaycalendar
        [HttpPost]
        public async Task<IActionResult> CreateHoliday([FromBody] HolidayCalendar holiday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.HolidayCalendars.Add(holiday);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Holiday created successfully" });
        }
    }

}
