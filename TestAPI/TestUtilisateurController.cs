using API.Controllers;
using API.Services.Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestAPI
{
    /// <summary>
    /// Classe de test pour UtilisateurController
    /// </summary>
    public class TestUtilisateurController
    {
        private readonly UtilisateurController controller;
        private readonly Mock<IUtilisateurService> mockService;

        public TestUtilisateurController()
        {
            mockService = new Mock<IUtilisateurService>();
            controller = new UtilisateurController(mockService.Object);
        }

        [Fact]
        public void TestRegister_Success()
        {
            // Arrange
            var input = new Utilisateur { Login = "A1", Mdp = "Test" };
            var output = new Utilisateur { Id = 1, Login = "A1" };
            mockService.Setup(s => s.Register(input)).Returns(output);

            // Act
            var result = controller.Register(input);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<Utilisateur>(createdResult.Value);
            Assert.Equal(1, returnedUser.Id);
            Assert.Equal("A1", returnedUser.Login);
        }

        [Fact]
        public void TestLogin_Success()
        {
            // Arrange
            var input = new Utilisateur { Login = "A1", Mdp = "Test" };
            var output = new Utilisateur { Id = 1, Login = "A1" };
            mockService.Setup(s => s.Login(input)).Returns(output);

            // Act
            var result = controller.Login(input);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<Utilisateur>(okResult.Value);
            Assert.Equal(1, returnedUser.Id);
            Assert.Equal("A1", returnedUser.Login);
        }

        [Fact]
        public void TestLogin_Fail()
        {
            // Arrange
            var input = new Utilisateur { Login = "A1", Mdp = "Wrong" };
            mockService.Setup(s => s.Login(input)).Returns((Utilisateur?)null);

            // Act
            var result = controller.Login(input);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result.Result);
        }
    }
}
