using System;

namespace EmployeeAttendanceManagment.Model
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public bool IsPresent { get; set; }  // Added IsPresent property

        public Employee Employee { get; set; }
    }
}
