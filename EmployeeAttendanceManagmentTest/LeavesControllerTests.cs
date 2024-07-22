using EmployeeAttendanceManagement.Model;
using EmployeeAttendanceManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeAttendanceManagement.Tests
{
    public class LeavesControllerTests
    {
        private readonly DbContextOptions<EmployeeManagementDbContext> _options;

        public LeavesControllerTests()
        {
            _options = new DbContextOptionsBuilder<EmployeeManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "EmployeeAttendanceDB")
                .Options;
        }

        [Fact]
        public async Task GetAvailableLeaves_ReturnsOkResult_WithAvailableLeaves()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.Leaves.Add(new Leave
                {
                    EmployeeID = 1,
                    CasualLeaves = 10,
                    TakenCasualLeaves = 3,
                    SickLeaves = 8,
                    TakenSickLeaves = 2,
                    PaidLeaves = 12,
                    TakenPaidLeaves = 4
                });
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new LeavesController(context);
                var model = new EmployeeIdModel { EmployeeID = 1 };

                // Act
                var result = await controller.GetAvailableLeaves(model);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var availableLeaves = okResult.Value;
                Assert.NotNull(availableLeaves);
            }
        }

        [Fact]
        public async Task GetAvailableLeaves_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new LeavesController(context);
                var model = new EmployeeIdModel { EmployeeID = 99 };

                // Act
                var result = await controller.GetAvailableLeaves(model);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal("No leaves found for this employee.", notFoundResult.Value);
            }
        }

        [Fact]
        public async Task GetTakenLeaves_ReturnsOkResult_WithTakenLeaves()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.Leaves.Add(new Leave
                {
                    EmployeeID = 1,
                    CasualLeaves = 10,
                    TakenCasualLeaves = 3,
                    SickLeaves = 8,
                    TakenSickLeaves = 2,
                    PaidLeaves = 12,
                    TakenPaidLeaves = 4
                });
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new LeavesController(context);
                var model = new EmployeeIdModel { EmployeeID = 1 };

                // Act
                var result = await controller.GetTakenLeaves(model);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var takenLeaves = okResult.Value;
                Assert.NotNull(takenLeaves);
            }
        }

        [Fact]
        public async Task GetTakenLeaves_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new LeavesController(context);
                var model = new EmployeeIdModel { EmployeeID = 99 };

                // Act
                var result = await controller.GetTakenLeaves(model);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal("No leaves found for this employee.", notFoundResult.Value);
            }
        }

        [Fact]
        public async Task TakeLeave_ReturnsOkResult_WhenLeaveRequestIsValid()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.Leaves.Add(new Leave
                {
                    EmployeeID = 1,
                    CasualLeaves = 10,
                    TakenCasualLeaves = 3,
                    SickLeaves = 8,
                    TakenSickLeaves = 2,
                    PaidLeaves = 12,
                    TakenPaidLeaves = 4
                });
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new LeavesController(context);
                var model = new TakeLeaveRequestModel { EmployeeID = 1, LeaveType = "casual", Days = 2 };

                // Act
                var result = await controller.TakeLeave(model);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var value = okResult.Value;

                // Use reflection to get the value of 'message' if it's an anonymous type
                var valueType = value.GetType();
                var messageProperty = valueType.GetProperty("message");
                if (messageProperty != null)
                {
                    var message = messageProperty.GetValue(value)?.ToString();
                    Assert.NotNull(message); // Ensure message is not null
                    Assert.Equal("Leave taken successfully", message);
                }
                else
                {
                    throw new Exception("The response object does not have a 'message' property.");
                }

                var updatedLeave = context.Leaves.FirstOrDefault(l => l.EmployeeID == 1);
                Assert.NotNull(updatedLeave);
                Assert.Equal(5, updatedLeave.TakenCasualLeaves);
            }
        }

        [Fact]
        public async Task TakeLeave_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new LeavesController(context);
                var model = new TakeLeaveRequestModel { EmployeeID = 99, LeaveType = "casual", Days = 2 };

                // Act
                var result = await controller.TakeLeave(model);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal("No leaves found for this employee.", notFoundResult.Value);
            }
        }

        [Fact]
        public async Task TakeLeave_ReturnsBadRequest_WhenNotEnoughLeavesAvailable()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.Leaves.Add(new Leave
                {
                    EmployeeID = 1,
                    CasualLeaves = 5,
                    TakenCasualLeaves = 4,
                    SickLeaves = 8,
                    TakenSickLeaves = 2,
                    PaidLeaves = 12,
                    TakenPaidLeaves = 4
                });
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new LeavesController(context);
                var model = new TakeLeaveRequestModel { EmployeeID = 1, LeaveType = "casual", Days = 2 };

                // Act
                var result = await controller.TakeLeave(model);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Not enough casual leaves available.", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task TakeLeave_ReturnsBadRequest_WhenInvalidLeaveType()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.Leaves.Add(new Leave
                {
                    EmployeeID = 1,
                    CasualLeaves = 10,
                    TakenCasualLeaves = 3,
                    SickLeaves = 8,
                    TakenSickLeaves = 2,
                    PaidLeaves = 12,
                    TakenPaidLeaves = 4
                });
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new LeavesController(context);
                var model = new TakeLeaveRequestModel { EmployeeID = 1, LeaveType = "invalid", Days = 2 };

                // Act
                var result = await controller.TakeLeave(model);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid leave type.", badRequestResult.Value);
            }
        }
    }
}
