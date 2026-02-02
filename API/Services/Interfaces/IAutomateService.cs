using LogicLayer;

namespace API.Services.Interfaces
{
    /// <summary>
    /// Interface définissant les opérations métiers pour la gestion des entités <see cref="Automate"/>.
    /// </summary>
    public interface IAutomateService
    {
        /// <summary>
        /// Récupération des automates
        /// </summary>
        /// <returns>Liste des automate</returns>
        /// <exception cref="DAOError">Si il y a un probleme avec le DAO</exception>
        List<Automate> GetAllAutomates();

        /// <summary>
        /// Récupération des automates d'un utilisateur
        /// </summary>
        /// <returns>Liste des automates</returns>
        /// <exception cref="DAOError">Si il y a un probleme avec le DAO</exception>
        List<Automate> GetAllAutomatesByUser(Utilisateur user);

        /// <summary>
        /// Récupération d'un automate
        /// </summary>
        /// <returns>Automate récupéré</returns>
        /// <exception cref="DAOError">Si il y a un probleme avec le DAO</exception>
        Automate GetAutomate(int id);

        /// <summary>
        /// Insertion d'un automate dans la base de donnée
        /// </summary>
        /// <param name="automate">Automate à ajouter</param>
        /// <returns>Automate avec l'id</returns>
        /// <exception cref="DAOError">Si il y a un probleme avec le DAO</exception>
        Automate AddAutomate(Automate automate);

        /// <summary>
        /// Mise à jour d'un automate dans la base de donnée
        /// </summary>
        /// <param name="automate">Automate à modifier</param>
        /// <returns>L'automate modifié</returns>
        /// <exception cref="DAOError">Si il y a un probleme avec le DAO</exception>
        Automate UpdateAutomate(Automate automate);

    }
}
