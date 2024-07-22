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
    public class HolidayCalendarControllerTests
    {
        private readonly DbContextOptions<EmployeeManagementDbContext> _options;

        public HolidayCalendarControllerTests()
        {
            _options = new DbContextOptionsBuilder<EmployeeManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "EmployeeAttendanceDB")
                .Options;
        }

        [Fact]
        public async Task GetHolidayCalendar_ReturnsOkResult_WithListOfHolidays()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.HolidayCalendars.AddRange(
                    new HolidayCalendar { Id = 1, Name = "New Year's Day", Date = new DateTime(2023, 1, 1) },
                    new HolidayCalendar { Id = 2, Name = "Christmas", Date = new DateTime(2023, 12, 25) }
                );
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new HolidayCalendarController(context);

                // Act
                var result = await controller.GetHolidayCalendar();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<HolidayCalendar>>(okResult.Value);
                Assert.Equal(2, returnValue.Count);
            }
        }

        [Fact]
        public async Task CreateHoliday_ReturnsOkResult_WhenHolidayIsValid()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new HolidayCalendarController(context);
                var holiday = new HolidayCalendar { Name = "Thanksgiving", Date = new DateTime(2023, 11, 23) };

                // Act
                var result = await controller.CreateHoliday(holiday);

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
                    Assert.Equal("Holiday created successfully", message);
                }
                else
                {
                    throw new Exception("The response object does not have a 'message' property.");
                }

                var createdHoliday = context.HolidayCalendars.FirstOrDefault(h => h.Name == "Thanksgiving");
                Assert.NotNull(createdHoliday);
                Assert.Equal("Thanksgiving", createdHoliday.Name);
            }
        }

        [Fact]
        public async Task CreateHoliday_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new HolidayCalendarController(context);
                controller.ModelState.AddModelError("Error", "Model state is invalid");

                // Act
                var result = await controller.CreateHoliday(new HolidayCalendar());

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }
    }
}
