using LogicLayer;

namespace API.Data.Interfaces
{
    /// <summary>
    /// Définit les opérations d'accès aux données pour les entités <see cref="Automate"/>.
    /// Cette interface décrit les méthodes permettant de récupérer, insérer
    /// et manipuler les automates stockés dans la base de données.
    /// </summary>
    public interface IAutomateDAO
    {
        /// <summary>
        /// Récupération des automates
        /// </summary>
        /// <returns>Liste des automate</returns>
        List<Automate> GetAllAutomates();

        /// <summary>
        /// Récupération des automates d'un utilisateur
        /// </summary>
        /// <returns>Liste des automates</returns>
        List<Automate> GetAllAutomatesByUser(Utilisateur user);

        /// <summary>
        /// Récupération d'un automate
        /// </summary>
        /// <param name="id">Identifiant unique de l’automate à rechercher.</param>
        /// <returns>Automate récupéré</returns>
        Automate GetAutomate(int id);

        /// <summary>
        /// Insertion d'un automate dans la base de donnée
        /// </summary>
        /// <param name="automate">Automate à ajouter</param>
        /// <returns>Automate avec l'id</returns>
        Automate AddAutomate(Automate automate);

    }
}
