using API.Data.Interfaces;
using API.Data.Realisations;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPI
{
    /// <summary>
    /// Classe de test pour la classe UtilisateurDAO
    /// </summary>
     public class TestUtilisateurSQLDAO
    {
        private IUtilisateurDAO dao;

        /// <summary>
        /// Constructeur
        /// </summary>
        public TestUtilisateurSQLDAO()
        {
            this.dao = new UtilisateurSQLDAO(new FakeBDDConnection());
        }

        [Fact]
        public void TestRegister()
        {
            // Arrange
            Utilisateur u = new Utilisateur
            {
                Login = "TestUser",
                Mdp = "TestPassword"
            };

            // Act
            u = this.dao.Register(u);

            // Assert
            Assert.NotNull(u);               // Id généré
            Assert.Equal("TestUser", u.Login);
            Assert.Equal("TestPassword", u.Mdp);
        }

        [Fact]
        public void TestGetByLogin()
        {
            // Arrange
            Utilisateur u = new Utilisateur
            {
                Login = "TestUser",
                Mdp = "TestPassword"
            };

            // Act
            u = this.dao.Register(u);
            u = this.dao.GetUserByLogin(u.Login);

            // Assert
            Assert.NotNull(u);
            Assert.NotNull(u.Id);               // Id généré
            Assert.Equal("TestUser", u.Login);
            Assert.Equal("TestPassword", u.Mdp);
        }




    }
}
