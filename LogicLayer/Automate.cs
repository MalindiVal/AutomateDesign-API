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

        
    }
}
