namespace EmployeeAttendanceManagment.Model
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int? ManagerID { get; set; }

        public Employee Manager { get; set; }
    }

}
