using LogicLayer;
using Service.Implementaions;
using Service.Interfaces;

namespace TestClientServices
{
    public class TestCsharpService
    {
        private ICSharpService service;
        public TestCsharpService()
        {
            service = new CSharpService();
        }

        [Fact]
        public void TestExport()
        {
            Automate automate = new Automate();
            automate.Nom = "TestDAO";
            automate.Utilisateur = new Utilisateur()
            {
                Id = 1,
                Login = "root"
            };
            Assert.Null(automate.Id);

            Etat e1 = new Etat { Nom = "Etat 1" };
            Etat e2 = new Etat { Nom = "Etat 2" };
            Etat e3 = new Etat { Nom = "Etat 3" };
            Etat e4 = new Etat { Nom = "Etat 4" };

            automate.Etats.Add(e1);
            automate.Etats.Add(e2);
            automate.Etats.Add(e3);
            automate.Etats.Add(e4);

            Assert.Contains(e1, automate.Etats);
            Assert.Contains(e2, automate.Etats);
            Assert.Contains(e3, automate.Etats);
            Assert.Contains(e4, automate.Etats);

            foreach (Etat etat in automate.Etats)
            {
                Random r = new Random();
                etat.Position.X = r.Next();
                etat.Position.Y = r.Next();
            }
            Transition t1 = new Transition(e1, e2) 
            { 
                Condition = "C1"
            };
            Transition t2 = new Transition(e2, e2)
            {
                Condition = "C2"
            };
            Transition t3 = new Transition(e3, e2)
            {
                Condition = "C3"
            };
            Transition t4 = new Transition(e1, e3)
            {
                Condition = "C4"
            };

            automate.Transitions.Add(t1);
            automate.Transitions.Add(t2);
            automate.Transitions.Add(t3);
            automate.Transitions.Add(t4);

            Assert.Contains(t1, automate.Transitions);
            Assert.Contains(t2, automate.Transitions);
            Assert.Contains(t3, automate.Transitions);
            Assert.Contains(t4, automate.Transitions);

            string temppath = Path.GetTempPath();
            service.ExportAutomate(automate, temppath);

            Assert.True(Path.IsPathRooted(temppath));
            Assert.True(Path.Exists(temppath));

            Assert.True(File.Exists(Path.Combine(temppath, "Etat.cs")));
            Assert.True(File.Exists(Path.Combine(temppath, "Evenement.cs")));
            Assert.True(File.Exists(Path.Combine(temppath, "Automate.cs")));

            foreach(Etat etat in automate.Etats)
            {
                string file = Path.Combine(temppath, "Etats", etat.Nom.Replace(" ", "_") + ".cs");
                Assert.True(File.Exists(file));
                File.Delete(file);
            }

            File.Delete(Path.Combine(temppath, "Etat.cs"));
            File.Delete(Path.Combine(temppath, "Evenement.cs"));
            File.Delete(Path.Combine(temppath, "Automate.cs"));

        }
    }
}