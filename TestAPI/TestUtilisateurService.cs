using API.Data.Interfaces;
using API.Data.Realisations;
using API.Services;
using API.Services.Interfaces;
using API.Services.Realisations;
using LogicLayer;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPI
{
    /// <summary>
    /// Classe de test pour la classe UtilisateurService
    /// </summary>
     public class TestUtilisateurService
    {
        private readonly Mock<IUtilisateurDAO> dao;
        private readonly Mock<IHasherPassword> hasher;
        private readonly IUtilisateurService service;

        /// <summary>
        /// Constructeur
        /// </summary>
        public TestUtilisateurService()
        {
            dao = new Mock<IUtilisateurDAO>();
            hasher = new Mock<IHasherPassword>();

            service = new UtilisateurService(dao.Object, hasher.Object);
        }

        [Fact]
        public void TestRegister()
        {
           Utilisateur u = new Utilisateur { Login = "Test", Mdp = "Test" };

            hasher.Setup(h => h.Hash("Test"))
                  .Returns("HASHED");

            dao.Setup(d => d.Register(It.IsAny<Utilisateur>()))
               .Returns<Utilisateur>(usr =>
               {
                   usr.Id = 1;
                   return usr;
               });

            // Act
            Utilisateur result = service.Register(u);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);

            hasher.Verify(h => h.Hash("Test"), Times.Once);
            dao.Verify(d => d.Register(It.IsAny<Utilisateur>()), Times.Once);
        }

        [Fact]
        public void TestLogin_Success()
        {
            // Arrange
            Utilisateur input = new Utilisateur { Login = "root", Mdp = "root" };

            Utilisateur stored = new Utilisateur
            {
                Id = 1,
                Login = "root",
                Mdp = "HASHED"
            };

            dao.Setup(d => d.GetUserByLogin("root")).Returns(stored);

            hasher.Setup(h => h.Verify("root", "HASHED")).Returns(true);

            Utilisateur result = service.Login(input);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void TestLogin_Fail()
        {
            // Arrange
            Utilisateur input = new Utilisateur { Login = "root", Mdp = "wrong" };

            Utilisateur stored = new Utilisateur
            {
                Id = 1,
                Login = "root",
                Mdp = "HASHED"
            };

            dao.Setup(d => d.GetUserByLogin("root"))
               .Returns(stored);

            hasher.Setup(h => h.Verify("wrong", "HASHED"))
                  .Returns(false);

            Utilisateur? result = service.Login(input);

            Assert.Null(result.Id);
        }
    }
}
