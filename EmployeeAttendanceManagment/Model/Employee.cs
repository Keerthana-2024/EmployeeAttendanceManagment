using EmployeeAttendanceManagement.Model;
using EmployeeAttendanceManagment.Controllers;
using System;
using System.Collections.Generic;

namespace EmployeeAttendanceManagment.Model
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string GeographicalArea { get; set; }
        public int DepartmentID { get; set; }
        public int AttendancePolicyID { get; set; } // New Foreign Key
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int? ManagerID { get; set; }
        public Department Department { get; set; }
        public AttendancePolicy AttendancePolicy { get; set; } // Navigation Property
        public Employee Manager { get; set; }
        public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
        public ICollection<Attendance> Attendances { get; set; }
    }


}
