using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ViewModels;
using Xunit;

namespace TestVM
{
    public class TestTransitionVM
    {

        /// <summary>
        /// Teste la création d'une transition entre deux états
        /// </summary>
        [Fact]
        public void Test_Creation_Transition()
        {
            Etat debut = new Etat();
            Etat fin = new Etat();
            EtatVM e1 = new EtatVM(debut);
            EtatVM e2 = new EtatVM(fin);
            TransitionVM vm = new TransitionVM(e1,e2);
            Assert.Equal(e1, vm.EtatDepart);
            Assert.Equal(e2, vm.EtatArrivee);
            Assert.NotNull(vm.Metier);
        }

    }
}
