using API.Controllers;
using API.Services.Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestAPI
{
    public class TestAutomateController
    {
        private readonly Mock<IAutomateService> mockService;
        private readonly AutomateController controller;

        public TestAutomateController()
        {
            mockService = new Mock<IAutomateService>();
            controller = new AutomateController(mockService.Object);
        }

        [Fact]
        public void TestGetAllAutomates()
        {
            List<Automate> automates = new List<Automate>
            {
                new Automate { Id = 1, Nom = "A1" },
                new Automate { Id = 2, Nom = "A2" }
            };
            mockService.Setup(s => s.GetAllAutomates()).Returns(automates);

            ActionResult<List<Automate>> result = controller.GetAllAutomates();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAutomates = Assert.IsType<List<Automate>>(okResult.Value);

            Assert.NotNull(returnedAutomates);
            Assert.Equal(2, returnedAutomates.Count);
            Assert.Equal("A1", returnedAutomates[0].Nom);
            Assert.Equal("A2", returnedAutomates[1].Nom);
        }

        [Fact]
        public void TestGetAllAutomates_EmptyList()
        {
            // Arrange
            mockService.Setup(s => s.GetAllAutomates()).Throws(new System.Exception());

            // Act
            var result = controller.GetAllAutomates();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
        }


        [Fact]
        public void TestGetAutomateById()
        {
            var automate = new Automate { Id = 1, Nom = "A1" };
            mockService.Setup(s => s.GetAutomate(1)).Returns(automate);

            var result = controller.GetAutomateById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAutomate = Assert.IsType<Automate>(okResult.Value);
            Assert.Equal(1, returnedAutomate.Id);
            Assert.Equal("A1", returnedAutomate.Nom);
        }


        [Fact]
        public void TestGetAutomateById_Null()
        {
            mockService.Setup(s => s.GetAutomate(1)).Throws(new System.Exception());

            var result = controller.GetAutomateById(1);

            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
        }


        [Fact]
        public void TestExportAutomate_Succes()
        {
            var input = new Automate { Nom = "A1" };
            var output = new Automate { Id = 1, Nom = "A1" };

            mockService.Setup(s => s.AddAutomate(input)).Returns(output);

            var result = controller.ExportAutomate(input);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedAutomate = Assert.IsType<Automate>(createdResult.Value);
            Assert.Equal(1, returnedAutomate.Id);
            Assert.Equal("A1", returnedAutomate.Nom);
        }

        [Fact]
        public void TestExportAutomate_Echec()
        {
            var input = new Automate { Nom = "A1" };
            mockService.Setup(s => s.AddAutomate(input)).Throws(new Exception());

            var result = controller.ExportAutomate(input);

            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
        }


    }
}
