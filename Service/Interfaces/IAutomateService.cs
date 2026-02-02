using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    /// <summary>
    /// Interface de service pour la gestion des importations et des exportations des automates
    /// </summary>
    public interface IAutomateService
    {
        /// <summary>
        /// Importe un automate depuis la base via son ID.
        /// </summary>
        /// <param name="id">ID de l’automate</param>
        public Task<Automate> ImporterAutomate(int id);

        /// <summary>
        /// Récupère tous les automates depuis le serveur.
        /// </summary>
        public Task<List<Automate>> GetAllAutomates();

        /// <summary>
        /// Récupère tous les automates d'un utilisateur depuis le serveur.
        /// </summary>
        public Task<List<Automate>> GetAllAutomatesByUser(Utilisateur user);

        /// <summary>
        /// Permet d'exporter un Automate dans la base de données
        /// </summary>
        /// <param name="automate">L'automate à transférer</param>
        /// <returns>L'automate avec un id attaché</returns>
        public Task<Automate> AddAutomate(Automate automate);

        /// <summary>
        /// Met à jour un automate existant dans l'API.
        /// </summary>
        /// <param name="automate">L'automate avec les nouvelles données</param>
        /// <returns>L'automate mis à jour</returns>
        public Task<Automate> UpdateAutomate(Automate automate);

        /// <summary>
        /// Sauvegarde d'un Automate dans un fichier
        /// </summary>
        /// <param name="metier">Automate à sauvegarder</param>
        /// <param name="filename">Chemin du fichier</param>
        public void SauvegardeAutomate(Automate metier,string filename);

        /// <summary>
        /// Permet de charger un automate à partir d'un fichier json
        /// </summary>
        /// <param name="filename">chemin du fichier à charger</param>
        /// <returns>Automate crée à partir du fichier</returns>
        public Automate ChargementAutomate(string filename);
    }
}
