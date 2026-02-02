using LogicLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Classe représentant un utilisateur
    /// </summary>
    public class Utilisateur
    {
        #region Attributs
        private int? id = null;
        private string? login;
        private string? mdp = null;
        #endregion

        #region Propriétés
        /// <summary>
        /// Identifiant d'un utilisateur
        /// </summary>
        public int? Id
        {
            get => id;
            set
            {
                // Vérification que l'id n'est pas négatif
                if (value < 0)
                {
                    throw new NoNegatifIdError();
                }
                else
                {
                    id = value;
                }
            }
        }
        /// <summary>
        /// Login de l'utilisateur
        /// </summary>
        public string? Login { 
            get => login; 
            set => login = value;
        }

        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        public string? Mdp
        {
            get => mdp;
            set => mdp = value;
        }
        #endregion
    }
}
