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
    public class TestEtatSQLDAO
    {
        private readonly IEtatDAO etatDAO;
        private Automate test;

        public TestEtatSQLDAO()
        {
            this.etatDAO = new EtatSQLDAO(new FakeBDDConnection());
        }

        private void CreationAutomateTest()
        {
            this.test = new Automate();
            this.test.Id = 1;
            this.test.Nom = "TestDAO_" + Guid.NewGuid().ToString("N");
            this.test.Utilisateur = new Utilisateur()
            {
                Id = 1,
                Login = "root"
            };
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
        }

        [Fact]
        public void TestInsertEtats()
        {
            CreationAutomateTest();
            this.etatDAO.InsertEtats(this.test);
            foreach (Etat e in this.test.Etats)
            {
                Assert.NotNull(e.Id);
            }
        }

        [Fact]
        public void TestGetEtatsByAutomate()
        {
            CreationAutomateTest();
            this.etatDAO.InsertEtats(this.test);
            Assert.NotNull(this.test.Id);
            List<Etat> etats = this.etatDAO.GetEtatsByAutomate((int)this.test.Id);
            Assert.Equal(this.test.Etats.Count, etats.Count);
            foreach (Etat e in this.test.Etats)
            { 
                Assert.NotNull(e.Id);
                Assert.Contains(etats, e => e.Id == e.Id && e.Nom == e.Nom);
            }
        }
    }
}
