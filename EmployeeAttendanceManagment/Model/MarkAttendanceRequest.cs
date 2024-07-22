using System;

namespace EmployeeAttendanceManagment.Model
{
    public class MarkAttendanceRequest
    {
        public int EmployeeID { get; set; }
        public string CurrentLocation { get; set; }
        public DateTime Date { get; set; }
    }
}
