using LogicLayer;

namespace API.Data.Interfaces
{
    /// <summary>
    /// Interface définissant les opérations d'accès aux données pour les entités <see cref="Utilisateur"/>.
    /// </summary>
    public interface IUtilisateurDAO
    {
        

        /// <summary>
        /// Permet d'enregistrer son compte dans la base de données
        /// </summary>
        /// <param name="user">objet utilisateur crée dans la page d'inscription</param>
        /// <returns>Données utilisateur</returns>
        public Utilisateur Register(Utilisateur user);

        /// <summary>
        /// Récupère un utilisateur via son login
        /// </summary>
        /// <param name="login">Le login de l'utilisateur à récupérer</param>
        /// <returns>Les données de l'utilisateur</returns>
        public Utilisateur? GetUserByLogin(string login);
    }
}
