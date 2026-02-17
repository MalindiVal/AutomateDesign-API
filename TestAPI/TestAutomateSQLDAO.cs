using API.Data.Interfaces;
using API.Data.Realisations;
using LogicLayer;

namespace TestAPI
{
    public class TestAutomateSQLDAO
    {
        private IAutomateDAO automateDAO;
        private Automate test;

        public TestAutomateSQLDAO()
        {
            this.automateDAO = new AutomateSQLDAO(new FakeBDDConnection());
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
            Random r = new Random();
            foreach (Etat etat in this.test.Etats)
            {
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
        public void TestCreationAutomate()
        {

            CreationAutomateTest();
            this.test = this.automateDAO.AddAutomate(this.test);
            Assert.NotNull(this.test.Id);

        }

        /// <summary>
        /// Test de la fonction de récupération des automates
        /// </summary>
        [Fact]
        public void TestListerTouslesAutomates()
        {
            CreationAutomateTest();
            this.test = this.automateDAO.AddAutomate(this.test);
            Assert.NotNull(this.test.Id);
            List<Automate> list = this.automateDAO.GetAllAutomates();
            Assert.NotEmpty(list);
        }

        /// <summary>
        /// Récupération des données d'un automate depuis la base de données
        /// </summary>
        [Fact]
        public void TestGetAutomateById()
        {
            CreationAutomateTest();

            this.test = this.automateDAO.AddAutomate(this.test);
            Assert.NotNull(this.test.Id);

            List<Automate> list = this.automateDAO.GetAllAutomates();

            Assert.NotEmpty(list);
            Assert.Contains(list, a => a.Id == this.test.Id);
            Assert.Contains(list, a => a.Nom == this.test.Nom);



            Automate res = this.automateDAO.GetAutomate((int)this.test.Id);
            Assert.NotNull(res);
            Assert.Equal(this.test.Id, res.Id);
            Assert.Equal(this.test.Nom, res.Nom);

        }

    }
}