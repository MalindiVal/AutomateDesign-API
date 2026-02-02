using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientData.Realisations
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

        #region Méthodes
        /// <summary>
        /// Création d'un DTO à partir d'un objet domaine.
        /// </summary>
        /// <param name="automate">L'automate de base.</param>
        /// <returns>Un automateDto fait à partir d'un Automate.</returns>
        public static AutomateDto FromDomain(Automate automate)
        {
            return new AutomateDto
            {
                Id = automate.Id,
                Nom = automate.Nom,
                Etats = automate.Etats.ToList(),
                Transitions = automate.Transitions.ToList(),
                Utilisateur = automate.Utilisateur
            };
        }

        /// <summary>
        /// Conversion du DTO vers l'objet domaine.
        /// </summary>
        /// <returns>Un Automate à partir de cette object.</returns>
        public Automate ToDomain()
        {
            var auto = new Automate
            {
                Id = this.Id,
                Nom = this.Nom,
                Utilisateur = this.Utilisateur
            };

            foreach (var e in Etats)
                auto.AjouterEtat(e);

            foreach (var t in Transitions)
                auto.AjouterTransition(t);

            return auto;
        }
        #endregion
    }
}
