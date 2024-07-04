namespace EmployeeAttendanceManagment.Model
{
    public class OrganizationChart
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int ManagerID { get; set; }

        public Employee Employee { get; set; }
        public Employee Manager { get; set; }
    }

}
