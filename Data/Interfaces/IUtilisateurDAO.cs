using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientData.Interfaces
{
    /// <summary>
    /// Interface définissant les opérations d'accès aux données pour les entités <see cref="Utilisateur"/>.
    /// </summary>
    public interface IUtilisateurDAO
    {
        /// <summary>
        /// Récupèration des données utilisateurs (SAUF DU MOT DE PASSE)
        /// </summary>
        /// <param name="user">objet utilisateur crée dans la page de connexion</param>
        /// <returns>Données utilisateur</returns>
        /// <exception cref="DAOError">Si il y a un probleme avec le DAO</exception>
        public Task<Utilisateur> Login(Utilisateur user);

        /// <summary>
        /// Permet d'enregistrer son compte dans la base de données
        /// </summary>
        /// <param name="user">objet utilisateur crée dans la page d'inscription</param>
        /// <returns>Données utilisateur</returns>
        /// <exception cref="DAOError">Si il y a un probleme avec le DAO</exception>
        public Task<Utilisateur> Register(Utilisateur user);
    }
}
