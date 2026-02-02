using LogicLayer;
using ViewModels;

namespace TestVM
{
    public class TestEtatVM
    {
        /// <summary>
        /// Test de la création d'un EtatVM
        /// </summary>
        [Fact]
        public void TestCreationEtatVM()
        {
            Etat e = new Etat();
            EtatVM vm = new EtatVM(e);
            Assert.Equal(e, vm.Metier);
            Assert.Equal(e.Nom, vm.Nom);
            Assert.Equal(e.Position.X,vm.X);
            Assert.Equal(e.Position.Y,vm.Y);

        }
    }
}