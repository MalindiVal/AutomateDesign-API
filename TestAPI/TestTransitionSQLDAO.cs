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
    public class TestTransitionSQLDAO
    {
        private readonly ITransitionDAO transitionDAO;
        private Automate test;

        public TestTransitionSQLDAO()
        {
            this.transitionDAO = new TransitionSQLDAO(new FakeBDDConnection());
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

        [Fact]
        public void TestInsertTransitions()
        {
            // ARRANGE
            CreationAutomateTest();
            test.Id = 1;

            int idEtat = 1;
            foreach (var etat in test.Etats)
            {
                etat.Id = idEtat++;
            }

            // ACT
            transitionDAO.InsertTransitions(this.test);
            Dictionary<int, Etat> valuePairs = new Dictionary<int, Etat>();
            foreach (var etat in this.test.Etats)
            {
                valuePairs[etat.Id] = etat;
            }
            var result = transitionDAO.GetTransitionsByAutomate((int)this.test.Id, valuePairs);

            // ASSERT
            Assert.NotEmpty(result);
            Assert.Equal(test.Transitions.Count, result.Count);
        }

        [Fact]
        public void TestInsertTransitionsWithInvalidEtat()
        {
            // ARRANGE
            CreationAutomateTest();
            test.Id = 0;
            // ACT & ASSERT
            Assert.Throws<DAOError>(() => transitionDAO.InsertTransitions(this.test));

        }

        [Fact]
        public void TestGetTransitionsByAutomateWithInvalidAutomateId()
        {
            // ARRANGE
            CreationAutomateTest();
            test.Id = 1;
            Dictionary<int, Etat> valuePairs = new Dictionary<int, Etat>();
            foreach (var etat in this.test.Etats)
            {
                valuePairs[etat.Id] = etat;
            }
            // ACT
            var result = transitionDAO.GetTransitionsByAutomate(-1, valuePairs);
            // ASSERT
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetTransitionsByAutomateWithEmptyEtatDictionary()
        {
            // ARRANGE
            CreationAutomateTest();
            test.Id = 1;
            // ACT
            var result = transitionDAO.GetTransitionsByAutomate((int)this.test.Id, new Dictionary<int, Etat>());
            // ASSERT
            Assert.Empty(result);
        }
    }
}
