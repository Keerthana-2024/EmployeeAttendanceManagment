namespace EmployeeAttendanceManagment.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}
