using EmployeeAttendanceManagement.Model;
using EmployeeAttendanceManagment.Controllers;
using EmployeeAttendanceManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceManagmentTest
{
    [TestFixture]
    public class EmployeesControllerTests
    {
        private Mock<EmployeeManagementDbContext> _contextMock;
        private EmployeesController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EmployeeManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _contextMock = new Mock<EmployeeManagementDbContext>(options);
            _controller = new EmployeesController(_contextMock.Object);
        }

        [Test]
        public async Task RegisterEmployee_ShouldReturnOk_WhenEmployeeIsRegistered()
        {
            // Arrange
            var employee = new Employee
            {
                Username = "testuser",
                PasswordHash = "testpassword"
            };

            var dbSetMock = new Mock<DbSet<Employee>>();
            var queryable = new[] { employee }.AsQueryable();
            dbSetMock.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            _contextMock.Setup(c => c.Employees).Returns(dbSetMock.Object);
            _contextMock.Setup(c => c.Employees.Add(It.IsAny<Employee>())).Verifiable();
            _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.RegisterEmployee(employee);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual("Employee registered successfully", ((dynamic)okResult.Value).message);
        }

        [Test]
        public async Task SignIn_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var login = new EmployeeLogin
            {
                Username = "testuser",
                Password = "testpassword"
            };

            var employee = new Employee
            {
                Username = "testuser",
                PasswordHash = HashPassword("testpassword")
            };

            var dbSetMock = new Mock<DbSet<Employee>>();
            var queryable = new[] { employee }.AsQueryable();
            dbSetMock.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            _contextMock.Setup(c => c.Employees).Returns(dbSetMock.Object);
            _contextMock.Setup(c => c.Employees.FirstOrDefaultAsync(It.IsAny<Expression<Func<Employee, bool>>>(), default))
                .ReturnsAsync(employee);

            // Act
            var result = await _controller.SignIn(login);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual("Sign-in successful", ((dynamic)okResult.Value).message);
            Assert.AreEqual(employee.EmployeeID, ((dynamic)okResult.Value).employeeId);
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

        // Example of password verification
        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInputPassword = HashPassword(password);
            return hashedInputPassword == hashedPassword;
        }
    }
}
