using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLogic
{
    public class TestTransition
    {
        /// <summary>
        /// Test de la création d'une transition
        /// </summary>
        [Fact]
        public void TestCréationTransition()
        {
            Etat e1 = new Etat();
            e1.EstFinal = true;
            Etat e2 = new Etat();
            Transition t = new Transition(e1,e2);
            Assert.False(t.EtatDebut.EstFinal);
        }

        /// <summary>
        /// Test du changement de l'état final
        /// </summary>
        [Fact]
        public void TestModificationEtatFinal()
        {
            Etat e1 = new Etat();
            Etat e2 = new Etat();
            Etat e3 = new Etat();
            e3.Nom = "Test";
            Transition t = new Transition(e1, e2);
            t.EtatFinal = e3;
            Assert.Equal("Test",t.EtatFinal.Nom);
        }

        /// <summary>
        /// Test du changement de l'etat de début 
        /// </summary>
        [Fact]
        public void TestModificationEtatdeDebut()
        {
            Etat e1 = new Etat();
            Etat e2 = new Etat();
            Etat e3 = new Etat();
            e3.Nom = "Test";
            Transition t = new Transition(e1, e2);
            t.EtatDebut = e3;
            Assert.Equal("Test", t.EtatDebut.Nom);
        }
    }
}
