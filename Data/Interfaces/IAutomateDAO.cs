using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientData.Interfaces
{
    /// <summary>
    /// Interface définissant les opérations d'accès aux données pour les entités <see cref="Automate"/>.
    /// </summary>
    public interface IAutomateDAO
    {
        /// <summary>
        /// Récupération des automates
        /// </summary>
        /// <returns>Liste des automate</returns>
        /// <exception cref="DAOError">Une erreur s'est produit dans le dao</exception>
        Task<List<Automate>> GetAllAutomates();

        /// <summary>
        /// Récupération des automates d'un utilisateur
        /// </summary>
        /// <param name="user">Utilisateur</param>
        /// <returns>Liste des automate</returns>
        /// <exception cref="DAOError">Une erreur s'est produit dans le dao</exception>
        Task<List<Automate>> GetAllAutomatesByUser(Utilisateur user);

        /// <summary>
        /// Récupération d'un automate
        /// </summary>
        /// <param name="id">Identifiant de l'automate recherché</param>
        /// <returns>Automate récupéré</returns>
        /// <exception cref="DAOError">Une erreur s'est produit dans le dao</exception>
        Task<Automate> GetAutomate(int id);

        /// <summary>
        /// Insertion d'un automate dans la base de donnée
        /// </summary>
        /// <param name="automate">Automate à ajouter</param>
        /// <returns>Automate avec l'id</returns>
        /// <exception cref="DAOError">Une erreur s'est produit dans le dao</exception>
        Task<Automate> AddAutomate(Automate automate);

        /// <summary>
        /// Mise à jour d'un automate dans la base de donnée
        /// </summary>
        /// <param name="automate">Automate à modifier</param>
        /// <returns>Automate modifié</returns>
        /// <exception cref="DAOError">Si il y a un probleme avec le DAO</exception>
        Task<Automate> UpdateAutomate(Automate automate);
    }
}
