using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    /// <summary>
    /// Interface de services pour l'envoi et la receptions des données utilisateurs
    /// </summary>
    public interface IUtilisateurService
    {
        /// <summary>
        /// Permet de créer un compte sur la base de données
        /// </summary>
        /// <param name="user">Utilisateur à inscrire</param>
        /// <returns>Utilisateur avec un identifiant</returns>
        public Task<Utilisateur> Register(Utilisateur user);

        /// <summary>
        /// Permet de faire une demande de connexion
        /// </summary>
        /// <param name="user">Utilisateur avec un login et un mot de passe</param>
        /// <returns>Utilisateur avec un identifiant</returns>
        public Task<Utilisateur> Login(Utilisateur user);
    }
}
