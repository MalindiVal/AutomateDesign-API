using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementaions
{
    /// <summary>
    /// DTO pour la sérialisation des automates.
    /// </summary>
    internal class AutomateDto
    {
        #region Propriétés
        /// <summary>
        /// Identifiant de l'automate.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Nom de l'automate.
        /// </summary>
        public string Nom { get; set; } = "Automate";

        /// <summary>
        /// Liste des états de l'automate.
        /// </summary>
        public List<Etat> Etats { get; set; } = new();
        /// <summary>
        /// Liste des transitions de l'automate.
        /// </summary>
        public List<Transition> Transitions { get; set; } = new();

        /// <summary>
        /// Utilisateur propriétaire de l'automate.
        /// </summary>
        public Utilisateur? Utilisateur { get; set; }
        #endregion
    }
}
