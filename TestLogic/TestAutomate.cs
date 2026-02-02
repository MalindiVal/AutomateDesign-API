using LogicLayer;
using LogicLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLogic
{
    public class TestAutomate
    {
        /// <summary>
        /// Test de creation d'un automate
        /// </summary>
        [Fact]
        public void TestCréerAutomate()
        {
            Automate automate = new Automate();
            Assert.Empty(automate.Etats);
            Assert.Empty(automate.Transitions);
        }

        /// <summary>
        /// Test d'ajout d'un etat dans l'automate
        /// </summary>
        [Fact]
        public void TestAjoutEtat()
        {
            Etat e1 = new Etat();
            Automate automate = new Automate();

            automate.Etats.Add(e1);

            Assert.Contains(e1, automate.Etats);
        }

        /// <summary>
        /// Test de sppresion d'un etat dans un automate
        /// </summary>
        [Fact]
        public void TestSuppressionEtat()
        {
            
            Automate automate = new Automate();

            Etat e1 = new Etat();
            e1.Nom = "E1";
            automate.Etats.Add(e1);
            Etat e2 = new Etat();
            automate.Etats.Add(e2);
            Etat e3 = new Etat();
            automate.Etats.Add(e3);
            Etat e4 = new Etat();
            automate.Etats.Add(e4);

            Assert.Contains(e1, automate.Etats);
            Assert.Contains(e2, automate.Etats);
            Assert.Contains(e3, automate.Etats);
            Assert.Contains(e4, automate.Etats);

            automate.SupprimerEtat(e1);

            Assert.DoesNotContain(e1, automate.Etats);
        }

        /// <summary>
        /// Suppression d'un etat lié à une transition
        /// </summary>
        /*[Fact]
        public void TestSuppressionEtatdansUneTransition()
        {

            Automate automate = new Automate();

            Etat e1 = new Etat();
            automate.Etats.Add(e1);
            Etat e2 = new Etat();
            automate.Etats.Add(e2);
            Etat e3 = new Etat();
            automate.Etats.Add(e3);
            Etat e4 = new Etat();
            automate.Etats.Add(e4);

            Assert.Contains(e1, automate.Etats);
            Assert.Contains(e2, automate.Etats);
            Assert.Contains(e3, automate.Etats);
            Assert.Contains(e4, automate.Etats);

            Transition t = new Transition(e1, e2);
            automate.Transitions.Add(t);
            Assert.Contains(t, automate.Transitions);

            automate.SupprimerEtat(e1);
            Assert.DoesNotContain(e1, automate.Etats);
            Assert.DoesNotContain(t,automate.Transitions);


        }*/

        /// <summary>
        /// Test de l'ajout d'une tansition dans l'automate
        /// </summary>
        [Fact]
        public void TestAjoutTransition()
        {
            
            Automate automate = new Automate();

            Etat e1 = new Etat();
            e1.Nom = "E1";
            automate.Etats.Add(e1);
            Assert.Contains(e1, automate.Etats);
            Etat e2 = new Etat();
            automate.Etats.Add(e2);
            Assert.Contains(e2, automate.Etats);

            Transition t = new Transition(e1,e2);
            automate.Transitions.Add(t);
            Assert.Contains(t,automate.Transitions);
        }


        /// <summary>
        /// Test de la suppresion d'une transition dans un automate
        /// </summary>
        [Fact]
        public void TestSuppressionTransition()
        {

            Automate automate = new Automate();

            Etat e1 = new Etat();
            automate.Etats.Add(e1);
            Assert.Contains(e1, automate.Etats);
            Etat e2 = new Etat();
            automate.Etats.Add(e2);
            Assert.Contains(e2, automate.Etats);
            Etat e3 = new Etat();
            automate.Etats.Add(e3);
            Assert.Contains(e3, automate.Etats);
            Etat e4 = new Etat();
            automate.Etats.Add(e4);
            Assert.Contains(e4, automate.Etats);
            Etat e5 = new Etat();
            automate.Etats.Add(e5);
            Assert.Contains(e5, automate.Etats);
            Etat e6 = new Etat();
            automate.Etats.Add(e6);
            Assert.Contains(e6, automate.Etats);
            Etat e7 = new Etat();
            automate.Etats.Add(e7);
            Assert.Contains(e7, automate.Etats);
            Etat e8 = new Etat();
            automate.Etats.Add(e8);
            Assert.Contains(e8, automate.Etats);

            Transition t = new Transition(e1, e2);
            automate.Transitions.Add(t);
            Assert.Contains(t, automate.Transitions);
            Transition t2 = new Transition(e3, e4);
            automate.Transitions.Add(t2);
            Assert.Contains(t2, automate.Transitions);
            Transition t3 = new Transition(e5, e6);
            automate.Transitions.Add(t3);
            Assert.Contains(t3, automate.Transitions);
            Transition t4 = new Transition(e7, e8);
            automate.Transitions.Add(t4);
            Assert.Contains(t4, automate.Transitions);

            automate.Transitions.Remove(t);

            Assert.DoesNotContain(t, automate.Transitions);
        }

        [Fact]
        public void TestNoNegatifId()
        {
            Automate automate = new Automate();
            Assert.Empty(automate.Etats);
            Assert.Empty(automate.Transitions);
            Assert.Throws<NoNegatifIdError>(() => { automate.Id = -1; });
            
        }
    }
}
