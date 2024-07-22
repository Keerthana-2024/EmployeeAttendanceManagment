using EmployeeAttendanceManagement.Model;
using EmployeeAttendanceManagment.Controllers;
using EmployeeAttendanceManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeAttendanceManagement.Tests
{
    public class OrganizationChartControllerTests
    {
        private readonly DbContextOptions<EmployeeManagementDbContext> _options;

        public OrganizationChartControllerTests()
        {
            _options = new DbContextOptionsBuilder<EmployeeManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "EmployeeAttendanceDB")
                .Options;
        }

        [Fact]
        public async Task GetOrganizationChart_ReturnsOkResult_WithOrganizationChartEntries()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                context.OrganizationCharts.Add(new OrganizationChart
                {
                    EmployeeID = 1,
                    ManagerID = 2
                });
                context.SaveChanges();
            }

            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new OrganizationChartController(context);

                // Act
                var result = await controller.GetOrganizationChart();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var organizationChartEntries = okResult.Value;
                Assert.NotNull(organizationChartEntries);
            }
        }

        [Fact]
        public async Task CreateOrganizationChart_ReturnsOkResult_WhenOrganizationChartIsValid()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new OrganizationChartController(context);
                var organizationChart = new OrganizationChart
                {
                    EmployeeID = 1,
                    ManagerID = 2
                };

                // Act
                var result = await controller.CreateOrganizationChart(organizationChart);

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
                    Assert.Equal("Organization chart entry created successfully", message);
                }
                else
                {
                    throw new Exception("The response object does not have a 'message' property.");
                }

                var createdEntry = context.OrganizationCharts.FirstOrDefault(oc => oc.EmployeeID == 1);
                Assert.NotNull(createdEntry);
            }
        }

        [Fact]
        public async Task CreateOrganizationChart_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            using (var context = new EmployeeManagementDbContext(_options))
            {
                var controller = new OrganizationChartController(context);
                controller.ModelState.AddModelError("EmployeeID", "Required");

                var organizationChart = new OrganizationChart
                {
                    ManagerID = 2
                };

                // Act
                var result = await controller.CreateOrganizationChart(organizationChart);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var modelState = badRequestResult.Value as SerializableError;
                Assert.NotNull(modelState);
                Assert.True(modelState.ContainsKey("EmployeeID"));
            }
        }
    }
}
