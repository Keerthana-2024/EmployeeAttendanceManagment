using Azure;
using EmployeeAttendanceManagement.Model;
using EmployeeAttendanceManagment.Controllers;
using EmployeeAttendanceManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeAttendanceManagmentTest
{
    public class EmployeesControllerTests
    {
        private readonly EmployeeManagementDbContext _context;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            var options = new DbContextOptionsBuilder<EmployeeManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "EmployeeAttendanceDB")
                .Options;

            _context = new EmployeeManagementDbContext(options);
            _controller = new EmployeesController(_context);
        }

        [Fact]
        public async Task RegisterEmployee_ShouldReturnOk_WhenEmployeeIsRegistered()
        {
            // Arrange
            var employee = new Employee
            {
                Username = "testuser",
                PasswordHash = "testpassword"
            };

            // Act
            var result = await _controller.RegisterEmployee(employee) as OkObjectResult;

            // Assert
            Assert.NotNull(result);

            // Check the type of result.Value
            Assert.IsType<OkObjectResult>(result);
            var value = result.Value;

            // Use reflection to get the value of 'message' if it's an anonymous type
            var valueType = value.GetType();
            var messageProperty = valueType.GetProperty("message");
            if (messageProperty != null)
            {
                var message = messageProperty.GetValue(value)?.ToString();
                Assert.NotNull(message); // Ensure message is not null
                Assert.Equal("Employee registered successfully", message);
            }
            else
            {
                throw new Exception("The response object does not have a 'message' property.");
            }

            // Verify employee is added to the database
            Assert.True(await _context.Employees.AnyAsync(e => e.Username == "testuser"));
        }


        [Fact]
        public async Task SignIn_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var employee = new Employee
            {
                Username = "testuser",
                PasswordHash = HashPassword("testpassword")
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var login = new EmployeeLogin
            {
                Username = "testuser",
                Password = "testpassword"
            };

            // Act
            var result = await _controller.SignIn(login) as OkObjectResult;

            // Assert
            Assert.NotNull(result);

            var response = result.Value;
            var valueType = response.GetType();

            // Use reflection to access properties dynamically

            // Get the 'message' property
            var messageProperty = valueType.GetProperty("message");
            Assert.NotNull(messageProperty);  // Ensure the property exists

            var message = messageProperty.GetValue(response);
            Assert.Equal("Sign-in successful", message);

            // Get the 'employeeId' property
            var employeeIdProperty = valueType.GetProperty("employeeId");
            Assert.NotNull(employeeIdProperty);  // Ensure the property exists

            var employeeId = employeeIdProperty.GetValue(response);
            Assert.Equal(employee.EmployeeID, employeeId);
        }

        // Example of a password hashing method
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }


    }
    public class SignInResponse
    {
        public string Message { get; set; }
        public int EmployeeId { get; set; }
    }

    public class RegisterEmployeeResponse
    {
        public string Message { get; set; }
    }

}
