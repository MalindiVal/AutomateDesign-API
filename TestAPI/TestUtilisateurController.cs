using API.Controllers;
using API.Services.Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestAPI
{
    public class TestUtilisateurController
    {
        private readonly Mock<IUtilisateurService> utilisateurServiceMock;
        private readonly Mock<ITokenService> tokenServiceMock;
        private readonly UtilisateurController controller;

        public TestUtilisateurController()
        {
            utilisateurServiceMock = new Mock<IUtilisateurService>();
            tokenServiceMock = new Mock<ITokenService>();

            controller = new UtilisateurController(
                utilisateurServiceMock.Object,
                tokenServiceMock.Object
            );
        }

        [Fact]
        public void Login_ReturnsOk_WhenValidCredentials()
        {
            // Arrange
            var loginUser = new Utilisateur { Login = "root", Mdp = "password" };
            var dbUser = new Utilisateur { Id = 1, Login = "root" };
            utilisateurServiceMock.Setup(s => s.Login(loginUser)).Returns(dbUser);
            tokenServiceMock.Setup(t => t.GenerateToken(dbUser)).Returns("fake-token");

            // Act
            var result = controller.Login(loginUser) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            dynamic value = result.Value;
            Assert.Equal("fake-token", value.token);
            Assert.Equal(1, value.user.Id);
            Assert.Equal("root", value.user.Login);
        }

        [Fact]
        public void Login_ReturnsUnauthorized_WhenWrongCredentials()
        {
            // Arrange
            var loginUser = new Utilisateur { Login = "root", Mdp = "wrong" };
            utilisateurServiceMock.Setup(s => s.Login(loginUser)).Returns((Utilisateur)null);

            // Act
            var result = controller.Login(loginUser) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenNullInput()
        {
            // Act
            var result = controller.Login(null) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Login_ThrowsInternalError_WhenServiceFails()
        {
            // Arrange
            var loginUser = new Utilisateur { Login = "root", Mdp = "password" };
            utilisateurServiceMock.Setup(s => s.Login(loginUser))
                .Throws(new System.Exception("DB error"));

            // Act
            var result = controller.Login(loginUser) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("DB error", result.Value.ToString());
        }

        [Fact]
        public void Register_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var newUser = new Utilisateur { Login = "newuser", Mdp = "password" };
            var createdUser = new Utilisateur { Id = 5, Login = "newuser" };
            utilisateurServiceMock.Setup(s => s.Register(newUser)).Returns(createdUser);
            tokenServiceMock.Setup(t => t.GenerateToken(createdUser)).Returns("token123");

            // Act
            var result = controller.Register(newUser) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            dynamic value = result.Value;
            Assert.Equal("token123", value.token);
            Assert.Equal(5, value.user.Id);
            Assert.Equal("newuser", value.user.Login);
        }

        [Fact]
        public void Register_ThrowsInternalError_WhenServiceFails()
        {
            // Arrange
            var newUser = new Utilisateur { Login = "newuser", Mdp = "password" };
            utilisateurServiceMock.Setup(s => s.Register(newUser))
                .Throws(new System.Exception("DB fail"));

            // Act
            var result = controller.Register(newUser) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("DB fail", result.Value.ToString());
        }
    }
}
