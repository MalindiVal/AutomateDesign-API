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
     public class TestUtilisateurDAO
    {
        private IUtilisateurDAO dao;

        /// <summary>
        /// Constructeur
        /// </summary>
        public TestUtilisateurDAO()
        {
            this.dao = new UtilisateurDAO();
        }

        [Fact]
        public void TestRegister()
        {
            Utilisateur u = new Utilisateur();
            u.Login = "Test";
            u.Mdp = "Test";

            u = this.dao.Register(u);
            Assert.NotNull(u);
        }

        
    }
}
