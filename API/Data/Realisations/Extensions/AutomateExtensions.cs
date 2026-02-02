using LogicLayer;

namespace API.Data.Realisations.Extensions
{
    /// <summary>
    /// Extensions pour la gestion des automates.
    /// </summary>
    internal static class AutomateExtensions
    {
        /// <summary>
        /// Supprime les états dupliqués dans un automate en s'assurant que chaque état est unique.
        /// </summary>
        /// <param name="automate">L'automate à vérifier</param>
        internal static void DeduplicateEtats(Automate automate)
        {
            foreach (var transition in automate.Transitions)
            {
                if (!automate.Etats.Contains(transition.EtatDebut))
                    automate.Etats.Add(transition.EtatDebut);
                if (!automate.Etats.Contains(transition.EtatFinal))
                    automate.Etats.Add(transition.EtatFinal);
            }

            var uniqueEtats = new HashSet<Etat>();
            var etatMap = new Dictionary<Etat, Etat>();

            foreach (var etat in automate.Etats)
            {
                if (uniqueEtats.TryGetValue(etat, out var existing))
                {
                    etatMap[etat] = existing;
                }
                else
                {
                    uniqueEtats.Add(etat);
                    etatMap[etat] = etat;
                }
            }

            foreach (var transition in automate.Transitions)
            {
                if (etatMap.TryGetValue(transition.EtatDebut, out var debut))
                    transition.EtatDebut = debut;
                if (etatMap.TryGetValue(transition.EtatFinal, out var fin))
                    transition.EtatFinal = fin;
            }

            automate.Etats = uniqueEtats.ToList();
        }
    }
}
