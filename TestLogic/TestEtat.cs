using LogicLayer;

namespace TestLogic
{
    public class TestEtat
    {

        /// <summary>
        /// Test du constructeur de l'objet etat
        /// </summary>
        [Fact]
        public void TestCreationEtat()
        {
            Etat e = new Etat();
            e.Nom = "Etat1";
            e.EstFinal = true;
        }
    }
}