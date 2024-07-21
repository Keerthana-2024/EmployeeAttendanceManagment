using EmployeeAttendanceManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly EmployeeManagementDbContext _context;

        public LeavesController(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        [HttpPost("available")]
        public async Task<IActionResult> GetAvailableLeaves([FromBody] EmployeeIdModel model)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(l => l.EmployeeID == model.EmployeeID);
            if (leave == null)
            {
                return NotFound("No leaves found for this employee.");
            }

            // Calculate available leaves
            var availableLeaves = new
            {
                CasualLeaves = leave.CasualLeaves - leave.TakenCasualLeaves,
                SickLeaves = leave.SickLeaves - leave.TakenSickLeaves,
                //ComboLeaves = leave.ComboLeaves - leave.TakenComboLeaves,
                PaidLeaves = leave.PaidLeaves - leave.TakenPaidLeaves
            };

            return Ok(availableLeaves);
        }

        [HttpPost("taken")]
        public async Task<IActionResult> GetTakenLeaves([FromBody] EmployeeIdModel model)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(l => l.EmployeeID == model.EmployeeID);
            if (leave == null)
            {
                return NotFound("No leaves found for this employee.");
            }

            var takenLeaves = new
            {
                CasualLeaves = leave.TakenCasualLeaves,
                SickLeaves = leave.TakenSickLeaves,
                //ComboLeaves = leave.TakenComboLeaves,
                PaidLeaves = leave.TakenPaidLeaves
            };

            return Ok(takenLeaves);
        }

        [HttpPost("take")]
        public async Task<IActionResult> TakeLeave([FromBody] TakeLeaveRequestModel model)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(l => l.EmployeeID == model.EmployeeID);
            if (leave == null)
            {
                return NotFound("No leaves found for this employee.");
            }

            switch (model.LeaveType.ToLower())
            {
                case "casual":
                    if (leave.CasualLeaves - leave.TakenCasualLeaves < model.Days)
                        return BadRequest("Not enough casual leaves available.");
                    leave.TakenCasualLeaves += model.Days;
                    break;
                case "sick":
                    if (leave.SickLeaves - leave.TakenSickLeaves < model.Days)
                        return BadRequest("Not enough sick leaves available.");
                    leave.TakenSickLeaves += model.Days;
                    break;
                //case "combo":
                //    if (leave.ComboLeaves - leave.TakenComboLeaves < model.Days)
                //        return BadRequest("Not enough combo leaves available.");
                //    leave.TakenComboLeaves += model.Days;
                //    break;
                case "paid":
                    if (leave.PaidLeaves - leave.TakenPaidLeaves < model.Days)
                        return BadRequest("Not enough paid leaves available.");
                    leave.TakenPaidLeaves += model.Days;
                    break;
                default:
                    return BadRequest("Invalid leave type.");
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Leave taken successfully" });
        }
    }

    public class EmployeeIdModel
    {
        public int EmployeeID { get; set; }
    }

    public class TakeLeaveRequestModel
    {
        public int EmployeeID { get; set; }
        public string LeaveType { get; set; }
        public int Days { get; set; }
    }
}
