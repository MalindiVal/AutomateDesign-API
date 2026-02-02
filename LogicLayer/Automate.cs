using LogicLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Représente un automate avec ses états et ses transitions.
    /// </summary>
    public class Automate
    {
        #region Attributs
        private int? id;
        private string nom = "Automate";
        private List<Etat> etats = new List<Etat>();
        private List<Transition> transitions = new List<Transition>();
        private Utilisateur? utilisateur;
        #endregion

        #region Propriétés
        /// <summary>
        /// Identifiant de l'automate
        /// </summary>
        /// /// <exception cref="NoNegatifIdError">Levée si la valeur est négative.</exception>
        public int? Id 
        { 
            get => id; 
            set 
            {
                // Vérification de la valeur avant l'assignation
                if (value < 0)
                {
                    throw new NoNegatifIdError();
                } else
                {
                    id = value;
                }
            } 
        }

        /// <summary>
        /// Nom de l'automate
        /// </summary>
        public string Nom 
        { 
            get => nom; 
            set => nom = value; 
        }

        /// <summary>
        /// Liste des états de l'automate
        /// </summary>
        public List<Etat> Etats
        {
            set => etats = value;
            get
            {
                return etats;
            }
        }

        /// <summary>
        /// Liste des transitions de l'automate
        /// </summary>
        public List<Transition> Transitions
        {
            set => transitions = value;
            get
            {
                return transitions;
            }
        }

        /// <summary>
        /// Utilisateur créateur de l'automate
        /// </summary>
        public Utilisateur? Utilisateur { get => utilisateur; set => utilisateur = value; }
        #endregion

        #region Methodes
        /// <summary>
        /// Supression d'un etat
        /// </summary>
        /// <param name="etat">Eta à supprimer</param>
        public void SupprimerEtat(Etat etat)
        {
            this.etats.Remove(etat);

            // On récupère toutes les transitions concernées
            List<Transition> aSupprimer = this.transitions.Where(t => t.EtatFinal == etat || t.EtatDebut == etat).ToList();

            // On les supprime ensuite
            foreach (Transition t in aSupprimer)
            {
                this.transitions.Remove(t);
            }
        }

        /// <summary>
        /// Ajout d'un état à l'automate.
        /// </summary>
        /// <param name="e">Etat à ajouter.</param>
        public void AjouterEtat(Etat e) => etats.Add(e);

        /// <summary>
        /// Ajout d'une transition à l'automate.
        /// </summary>
        /// <param name="t">Transition à ajouter.</param>
        public void AjouterTransition(Transition t) => transitions.Add(t);

        /// <summary>
        /// Suppression d'une transition de l'automate.
        /// </summary>
        /// <param name="t">Transition à supprimer.</param>
        public void SupprTransition(Transition t) => transitions.Remove(t);

        /// inheritdoc/>
        public override bool Equals(object? obj)
        {
            bool res;

            if (obj is not Automate other)
            {
                res = false;
            }
            else
            {
                res = Id == other.Id &&
                   Nom == other.Nom &&
                   Etats.SequenceEqual(other.Etats) &&
                   Transitions.SequenceEqual(other.Transitions);
            }
            return res;
        }
        #endregion
    }
}
