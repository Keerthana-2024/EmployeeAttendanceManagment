using EmployeeAttendanceManagment.Model;
using System;

public class Leave
{
    public int LeaveID { get; set; }
    public int EmployeeID { get; set; }
    public int CasualLeaves { get; set; } = 15;
    public int SickLeaves { get; set; } = 12;
    //public int ComboLeaves { get; set; } = 10;
    public int PaidLeaves { get; set; } = 20;
    public int TakenCasualLeaves { get; set; } = 0;
    public int TakenSickLeaves { get; set; } = 0;
   // public int TakenComboLeaves { get; set; } = 0;
    public int TakenPaidLeaves { get; set; } = 0;

    // Remove read-only properties if you will update the values directly
    // public int AvailableCasualLeaves => CasualLeaves - TakenCasualLeaves;
    // public int AvailableSickLeaves => SickLeaves - TakenSickLeaves;
    // public int AvailableComboLeaves => ComboLeaves - TakenComboLeaves;
    // public int AvailablePaidLeaves => PaidLeaves - TakenPaidLeaves;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Employee Employee { get; set; }
}
