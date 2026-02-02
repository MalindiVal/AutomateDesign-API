using LogicLayer;

namespace API.Services.Interfaces
{
    /// <summary>
    /// Interface définissant les opérations métiers pour la gestion des entités <see cref="Utilisateur"/>.
    /// </summary>
    public interface IUtilisateurService
    {
        /// <summary>
        /// Récupèration des données utilisateurs (SAUF DU MOT DE PASSE)
        /// </summary>
        /// <param name="user">objet utilisateur crée dans la page de connexion</param>
        /// <returns>Données utilisateur</returns>
        public Utilisateur Login(Utilisateur user);

        /// <summary>
        /// Permet d'enregistrer son compte dans la base de données
        /// </summary>
        /// <param name="user">objet utilisateur crée dans la page d'inscription</param>
        /// <returns>Données utilisateur</returns>
        public Utilisateur Register(Utilisateur user);
    }
}
