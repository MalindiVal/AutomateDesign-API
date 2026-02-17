using API.Controllers;
using API.Services.Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace TestAPI
{
    public class TestAutomateController
    {
        private readonly Mock<IAutomateService> serviceMock;
        private readonly AutomateController controller;

        public TestAutomateController()
        {
            serviceMock = new Mock<IAutomateService>();
            controller = new AutomateController(serviceMock.Object);

            // Simuler un utilisateur connecté
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "root"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public void GetAllAutomates_ReturnsOk_WithList()
        {
            // Arrange
            var data = new List<Automate>
            {
                new Automate { Id = 1, Nom = "A1" },
                new Automate { Id = 2, Nom = "A2" }
            };
            serviceMock.Setup(s => s.GetAllAutomates()).Returns(data);

            // Act
            var result = controller.GetAllAutomates() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(data, result.Value);
        }

        [Fact]
        public void GetAllAutomatesByUser_ReturnsOk_WithUserAutomates()
        {
            // Arrange
            var user = new Utilisateur { Id = 1, Login = "root" };
            var data = new List<Automate>
            {
                new Automate { Id = 1, Nom = "A1", Utilisateur = user },
                new Automate { Id = 2, Nom = "A2", Utilisateur = user }
            };
            serviceMock.Setup(s => s.GetAllAutomatesByUser(user)).Returns(data);

            // Act
            var result = controller.GetAllAutomatesByUser() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(data, result.Value);
        }

        [Fact]
        public void GetAutomateById_ReturnsOk_WhenFound()
        {
            // Arrange
            var automate = new Automate { Id = 10, Nom = "Test4" };
            serviceMock.Setup(s => s.GetAutomate(10)).Returns(automate);

            // Act
            var result = controller.GetAutomateById(10) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(automate, result.Value);
        }

        [Fact]
        public void GetAutomateById_ReturnsNotFound_WhenNull()
        {
            // Arrange
            serviceMock.Setup(s => s.GetAutomate(99)).Returns((Automate)null);

            // Act
            var result = controller.GetAutomateById(99) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void ExportAutomate_ReturnsCreated_WhenValid()
        {
            // Arrange
            var automate = new Automate { Nom = "NewAutomate" };
            var created = new Automate { Id = 5, Nom = "NewAutomate" };
            serviceMock.Setup(s => s.AddAutomate(It.IsAny<Automate>())).Returns(created);

            // Act
            var result = controller.ExportAutomate(automate) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(created, result.Value);
        }

        [Fact]
        public void ExportAutomate_ReturnsBadRequest_WhenNull()
        {
            // Act
            var result = controller.ExportAutomate(null) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void UpdateAutomate_ReturnsOk_WhenValid()
        {
            // Arrange
            var automate = new Automate { Id = 1, Nom = "Old" };
            var updated = new Automate { Id = 1, Nom = "Updated" };
            serviceMock.Setup(s => s.UpdateAutomate(It.IsAny<Automate>())).Returns(updated);

            // Act
            var result = controller.UpdateAutomate(automate) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(updated, result.Value);
        }

        [Fact]
        public void UpdateAutomate_ReturnsBadRequest_WhenNull()
        {
            // Act
            var result = controller.UpdateAutomate(null) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
    }
}
