using EmployeeAttendanceManagement.Model;
using EmployeeAttendanceManagment.Controllers;
using EmployeeAttendanceManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeAttendanceManagment.Tests
{
    public class AttendanceControllerTests
    {
        private readonly DbContextOptions<EmployeeManagementDbContext> _options;

        public AttendanceControllerTests()
        {
            _options = new DbContextOptionsBuilder<EmployeeManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "EmployeeAttendanceDB")
                .Options;
        }

        [Fact]
        public async Task GetAttendancePolicies_ReturnsOkResult_WithListOfPolicies()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.AttendancePolicies.AddRange(
                    new AttendancePolicy { PolicyID = 1, PolicyName = "Policy1", AllowedLocations = "Location1" },
                    new AttendancePolicy { PolicyID = 2, PolicyName = "Policy2", AllowedLocations = "Location2" }
                );
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new AttendanceController(context);

                // Act
                var result = await controller.GetAttendancePolicies();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<AttendancePolicy>>(okResult.Value);
                Assert.Equal(2, returnValue.Count);
            }
        }

        [Fact]
        public async Task CreateAttendancePolicy_ReturnsOkResult_WhenPolicyIsValid()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new AttendanceController(context);
                var policy = new AttendancePolicy { PolicyName = "Policy1", AllowedLocations = "Location1" };

                // Act
                var result = await controller.CreateAttendancePolicy(policy);

                // Assert
                Assert.NotNull(result);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var value = okResult.Value;

                // Use reflection to get the value of 'message' if it's an anonymous type
                var valueType = value.GetType();
                var messageProperty = valueType.GetProperty("message");
                if (messageProperty != null)
                {
                    var message = messageProperty.GetValue(value)?.ToString();
                    Assert.NotNull(message); // Ensure message is not null
                    Assert.Equal("Attendance policy created successfully", message);
                }
                else
                {
                    throw new Exception("The response object does not have a 'message' property.");
                }

                var createdPolicy = context.AttendancePolicies.FirstOrDefault(p => p.PolicyName == "Policy1");
                Assert.NotNull(createdPolicy);
                Assert.Equal("Policy1", createdPolicy.PolicyName);
            }
        }

        [Fact]
        public async Task CreateAttendancePolicy_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new AttendanceController(context);
                controller.ModelState.AddModelError("Error", "Model state is invalid");

                // Act
                var result = await controller.CreateAttendancePolicy(new AttendancePolicy());

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public async Task MarkAttendance_ReturnsOkResult_WhenAttendanceIsMarkedSuccessfully()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.Employees.Add(new Employee { EmployeeID = 1, AttendancePolicyID = 1 });
                context.AttendancePolicies.Add(new AttendancePolicy { PolicyID = 1, AllowedLocations = "Location1" });
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new AttendanceController(context);
                var model = new MarkAttendanceRequest { EmployeeID = 1, CurrentLocation = "Location1" };

                // Act
                var result = await controller.MarkAttendance(model);

                // Assert
                Assert.NotNull(result);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var value = okResult.Value;

                // Use reflection to get the value of 'message' if it's an anonymous type
                var valueType = value.GetType();
                var messageProperty = valueType.GetProperty("message");
                if (messageProperty != null)
                {
                    var message = messageProperty.GetValue(value)?.ToString();
                    Assert.NotNull(message); // Ensure message is not null
                    Assert.Equal("Attendance marked successfully.", message);
                }
                else
                {
                    throw new Exception("The response object does not have a 'message' property.");
                }

                var attendance = context.Attendances.FirstOrDefault(a => a.EmployeeID == 1);
                Assert.NotNull(attendance);
                Assert.Equal("Location1", attendance.Location);
            }
        }

        [Fact]
        public async Task MarkAttendance_ReturnsNotFound_WhenEmployeeNotFound()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new AttendanceController(context);
                var model = new MarkAttendanceRequest { EmployeeID = 1, CurrentLocation = "Location1" };

                // Act
                var result = await controller.MarkAttendance(model);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal("Employee not found.", notFoundResult.Value);
            }
        }

        [Fact]
        public async Task MarkAttendance_ReturnsNotFound_WhenPolicyNotFound()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.Employees.Add(new Employee { EmployeeID = 1, AttendancePolicyID = 1 });
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new AttendanceController(context);
                var model = new MarkAttendanceRequest { EmployeeID = 1, CurrentLocation = "Location1" };

                // Act
                var result = await controller.MarkAttendance(model);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal("Attendance policy not found.", notFoundResult.Value);
            }
        }

        [Fact]
        public async Task MarkAttendance_ReturnsBadRequest_WhenLocationNotAllowed()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.Employees.Add(new Employee { EmployeeID = 1, AttendancePolicyID = 1 });
                context.AttendancePolicies.Add(new AttendancePolicy { PolicyID = 1, AllowedLocations = "Location1" });
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new AttendanceController(context);
                var model = new MarkAttendanceRequest { EmployeeID = 1, CurrentLocation = "Location2" };

                // Act
                var result = await controller.MarkAttendance(model);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Location not allowed by the policy.", badRequestResult.Value);
            }
        }
    }
}
