using LogicLayer;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementaions
{
    /// <summary>
    /// Implémentation de l'interface ICsharpService
    /// </summary>
    public class CSharpService : ICSharpService
    {
        /// <inheritdoc/>
        public void ExportAutomate(Automate automate, string filepath)
        {

            List<string> evenements = new List<string>();
            Dictionary<string, Dictionary<string, string>> transitions = new Dictionary<string, Dictionary<string, string>>();
            string etatinit = "";
            foreach (Transition transition in automate.Transitions)
            {
                string evenement = transition.Condition.Replace(" ", "_");
                string etatDebut = transition.EtatDebut.Nom.Replace(" ", "_");
                string etatFinal = transition.EtatFinal.Nom.Replace(" ", "_");

                evenements.Add(evenement);

                if (!transitions.ContainsKey(etatDebut))
                    transitions[etatDebut] = new Dictionary<string, string>();

                transitions[etatDebut].Add(evenement, etatFinal);
            }

            foreach (Etat etat in automate.Etats)
            {
                string nomEtat = etat.Nom.Replace(" ", "_");
                if (!transitions.ContainsKey(nomEtat))
                    transitions[nomEtat] = new Dictionary<string, string>();
                if (etat.EstInitial)
                {
                    etatinit = nomEtat;
                }
            }

            this.GenererEvenement(evenements, filepath);
            this.GenererEtatAbstraite(filepath);
            this.GenererEtats(transitions, filepath);
            this.GenererAutomate(filepath, etatinit);

        }

        private void GenererEvenement(List<string> evenements, string filepath)
        {
            evenements = evenements.Distinct().ToList();

            using (FileStream fs = File.Create(Path.Combine(filepath, "Evenement.cs")))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("public enum Evenement");
                sw.WriteLine("{");
                foreach (string evt in evenements)
                {
                    sw.WriteLine($"{evt},");
                }
                sw.WriteLine("}");
            }
        }

        private void GenererAutomate(string filepath, string etatinit)
        {
            using (FileStream fs = File.Create(Path.Combine(filepath, "Automate.cs")))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(
                    $@"public class Automate
                    {{
                        private Etat etatCourant;

                        public Automate(Maison metier)
                        {{
                            this.etatCourant = new {etatinit}(metier);
                        }}

                        public void Activer(Evenement e)
                        {{
                            etatCourant.Action(e);
                            etatCourant = etatCourant.Transition(e);
                        }}
                    }}");
            }
        }



        private void GenererEtatAbstraite(string filepath)
        {
            using (FileStream fs = File.Create(Path.Combine(filepath, "Etat.cs")))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(
                @"
                abstract public  class Etat {   
                    
                    private Maison metier;

                    public Maison Metier { get => metier; }

                    public Etat(Maison metier)
                    {
                        this.metier = metier;
                    }
                     /// <summary>
                     /// Transition vers un nouvel état
                     /// </summary>
                     /// <param name=""e"">Événement déclencheur de la transition</param>
                     /// <returns>Nouvel état</returns>
                     public abstract Etat Transition(Evenement e);

                     /// <summary>
                     /// Action exécutée suite à un événement
                     /// </summary>
                     /// <param name=""e"">Événement déclencheur de l'action</param>
                     public abstract void Action(Evenement e);
                }"
                );
            }
        }

        private void GenererEtats(Dictionary<string, Dictionary<string, string>> etats, string filepath)
        {
            DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(filepath , "Etats"));
            foreach (string etatbase in etats.Keys)
            {
                string fichier = Path.Combine(dir.FullName, etatbase + ".cs");

                using (FileStream fs = File.Create(fichier))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    string evenements = "";

                    foreach (KeyValuePair<string,string> evt in etats[etatbase])
                    {
                        evenements += $"case Evenement.{evt.Key}: return new {evt.Value}(metier); break;";
                    }
                    sw.WriteLine(
                        $@"
                        public class {etatbase} : Etat
                        {{
                            public {etatbase}(Maison metier) : base(metier)
                            {{
                            }}

                            public override Etat Transition(Evenement e)
                            {{
                                switch (e)
                                {{
                                    {evenements}
                                    default : return this; break;
                                }}
                            }}

                            public override void Action(Evenement e)
                            {{
                                switch (e)
                                {{
                                    

                                }}
                            }}
                        }}");
                }
            }
        }

    }
}