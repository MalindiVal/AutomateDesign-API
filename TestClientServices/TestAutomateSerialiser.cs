using LogicLayer;
using Service.Implementaions;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClientServices
{
    public class TestJsonAutomateSerialiser
    {
        private IAutomateSerializer automateSerializer;
        public TestJsonAutomateSerialiser()
        {
            automateSerializer = new JsonAutomateSerializer();
        }
        [Fact]
        public void TestSauvegarde()
        {
            string file = Directory.GetCurrentDirectory() + "/test.json";
            Automate automate = new Automate();

            CréerAutomate(automate);

            automateSerializer.SauvegardeAutomate(automate, file);
            Assert.True(File.Exists(file));
            if (File.Exists(file))
                File.Delete(file);

        }

        [Fact]
        public void TestChargement()
        {
            string file = Directory.GetCurrentDirectory() + "/test.json";
            Automate automate = new Automate();
            automate.Nom = "Test Automate1";

            CréerAutomate(automate);

            automateSerializer.SauvegardeAutomate(automate, file);
            Assert.True(File.Exists(file));
            Automate res = null;
            res = automateSerializer.ChargementAutomate(file);
            Assert.NotNull(res);
            Assert.NotEmpty(res.Etats);
            Assert.NotEmpty(res.Transitions);

            if (File.Exists(file))
                File.Delete(file);


        }
        private void CréerAutomate(Automate automate)
        {
            for (int i = 0; i < 1000; i++)
            {
                Etat e1 = new Etat { Nom = "Etat " + i };
                automate.Etats.Add(e1);
            }

            for (int j = 0; j < automate.Etats.Count() - 1; j++)
            {
                Transition t = new Transition(automate.Etats[j], automate.Etats[j + 1]);
                automate.Transitions.Add(t);
            }
        }
    }
}
