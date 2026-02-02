using API.Data.Interfaces;
using API.Data.Realisations;
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
    public class TestAutomateService
    {

        private readonly Mock<IAutomateDAO> dao;
        private readonly IAutomateService automateService;
        private Automate test;

        public TestAutomateService()
        {
            dao = new Mock<IAutomateDAO>();

            automateService = new AutomateService(dao.Object);
        }
        private void CreationAutomateTest()
        {
            this.test = new Automate();
            this.test.Nom = "TestDAO_" + Guid.NewGuid().ToString("N");
            this.test.Utilisateur = new Utilisateur()
            {
                Id = 1,
                Login = "root"
            };
            Assert.Null(this.test.Id);

            Etat e1 = new Etat { Nom = "Etat1" };
            Etat e2 = new Etat { Nom = "Etat2" };
            Etat e3 = new Etat { Nom = "Etat3" };
            Etat e4 = new Etat { Nom = "Etat4" };

            this.test.Etats.Add(e1);
            this.test.Etats.Add(e2);
            this.test.Etats.Add(e3);
            this.test.Etats.Add(e4);

            Assert.Contains(e1, this.test.Etats);
            Assert.Contains(e2, this.test.Etats);
            Assert.Contains(e3, this.test.Etats);
            Assert.Contains(e4, this.test.Etats);

            foreach (Etat etat in this.test.Etats)
            {
                Random r = new Random();
                etat.Position.X = r.Next();
                etat.Position.Y = r.Next();
            }
            Transition t = new Transition(e1, e2);
            this.test.Transitions.Add(t);
            Assert.Contains(t, this.test.Transitions);
        }

        /// <summary>
        /// Test de l'insertion d'un automate dans la base de données
        /// </summary>
        [Fact]
        public void TestCreateAutomate()
        {
            CreationAutomateTest();

            dao.Setup(x => x.AddAutomate(It.IsAny<Automate>()))
               .Returns((Automate a) =>
               {
                   a.Id = 1;
                   return a;
               });

            // ACT
            Automate result = automateService.AddAutomate(this.test);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.NotEmpty(result.Etats);
            Assert.NotEmpty(result.Transitions);
            dao.Verify(x => x.AddAutomate(It.IsAny<Automate>()), Times.Once);
        }


        /// <summary>
        /// Test de la fonction de récupération des automates
        /// </summary>
        [Fact]
        public void TestGetAllAutomates()
        {

            CreationAutomateTest();
            List<Automate> data = new List<Automate>
            {
                new Automate { Id = 1, Nom = "Test" },
                new Automate { Id = 2, Nom = "Test2" },
                new Automate { Id = 3, Nom = "Test3" }
            };

            dao.Setup(x => x.GetAllAutomates()).Returns(data);

            var result = automateService.GetAllAutomates();

            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, a => a.Nom == "Test");
            Assert.Contains(result, a => a.Nom == "Test2");
            Assert.Contains(result, a => a.Nom == "Test3");
        }

        /// <summary>
        /// Tet du getAllAutomatesByUser
        /// </summary>
        [Fact]
        public void TestGetAllAutomatesByUser()
        {

            CreationAutomateTest();
            List<Automate> data = new List<Automate>
            {
                new Automate { Id = 1, Nom = "Test" ,Utilisateur = this.test.Utilisateur},
                new Automate { Id = 2, Nom = "Test2" ,Utilisateur = this.test.Utilisateur},
                new Automate { Id = 3, Nom = "Test3" ,Utilisateur = this.test.Utilisateur},

            };

            dao.Setup(x => x.GetAllAutomatesByUser(this.test.Utilisateur)).Returns(data);

            List<Automate> result = automateService.GetAllAutomatesByUser(this.test.Utilisateur);

            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, a => a.Nom == "Test");
            Assert.Contains(result, a => a.Nom == "Test2");
            Assert.Contains(result, a => a.Nom == "Test3");

            Assert.Contains(result, a => a.Utilisateur == this.test.Utilisateur);
        }


        /// <summary>
        /// Récupération des données d'un automate depuis la base de données
        /// </summary>
        [Fact]
        public void TestGetAutomateById()
        {
            Automate automate = new Automate
            {
                Id = 10,
                Nom = "Test4",
                Etats =
        {
            new Etat { Id = 1, Nom = "Etat1" },
            new Etat { Id = 2, Nom = "Etat2" }
        },
                Transitions =
        {
            new Transition(new Etat(), new Etat()) { Condition = "Test4" }
        }
            };

            dao.Setup(x => x.GetAutomate(10)).Returns(automate);

            var result = automateService.GetAutomate(10);

            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal("Test4", result.Nom);
            Assert.Equal(2, result.Etats.Count);
            Assert.Single(result.Transitions);
        }


        [Fact]
        public void TestExceptionDAOError()
        {



        }
    }
}
