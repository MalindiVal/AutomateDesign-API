using Client.Data;
using ClientData.Interfaces;
using LogicLayer;
using Service.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Http.Json;
using System.Text.Json;

namespace Service.Implementaions
{
    /// <summary>
    /// Implementation de l'interface IAutomateService
    /// </summary>
    public class AutomateService : IAutomateService
    {
        private readonly IAutomateDAO automateDAO;
        private readonly IAutomateSerializer automateSerializer;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="automateDAO">Dao pour se connecter à l'api pour gerer les automates en réseau</param>
        /// <param name="automateSerializer">Un objet qui s'ocuppe de la sérialisation et de la désérialisation de l'automate en un fichier</param>
        public AutomateService(IAutomateDAO automateDAO, IAutomateSerializer automateSerializer)
        {
            this.automateDAO = automateDAO;
            this.automateSerializer = automateSerializer;
        }

        #region DAO
        /// <summary>
        /// Importe un automate depuis la base via son ID.
        /// </summary>
        /// <param name="id">ID de l’automate</param>
        public async Task<Automate> ImporterAutomate(int id)
        {
            Automate metier = new Automate();
            metier = await automateDAO.GetAutomate(id);
            return metier;
        }

        /// <summary>
        /// Récupère tous les automates depuis le serveur.
        /// </summary>
        public async Task<List<Automate>> GetAllAutomates()
        {
            return await automateDAO.GetAllAutomates();
        }

        public async Task<List<Automate>> GetAllAutomatesByUser(Utilisateur user)
        {
            List<Automate> list = new List<Automate>();
            list = await automateDAO.GetAllAutomatesByUser(user);
            return list;
        }

        /// <summary>
        /// Permet d'exporter un Automate dans la base de données
        /// </summary>
        /// <param name="automate">L'automate à transférer</param>
        /// <returns>L'automate avec un id attaché</returns>
        public async Task<Automate> AddAutomate(Automate automate)
        {
            Automate metier = new Automate();
            metier = await automateDAO.AddAutomate(automate);
            return metier;
        }

        /// <summary>
        /// Met à jour un automate existant dans l'API.
        /// </summary>
        /// <param name="automate">L'automate avec les nouvelles données</param>
        /// <returns>L'automate mis à jour</returns>
        public async Task<Automate> UpdateAutomate(Automate automate)
        {
            return await this.automateDAO.UpdateAutomate(automate);
        }
        #endregion

        #region Serialisation

        /// <summary>
        /// Permet de sauvegarder
        /// </summary>
        /// <param name="metier">Automate à sauvegarder</param>
        public void SauvegardeAutomate(Automate metier,string filename)
        {
            automateSerializer.SauvegardeAutomate(metier, filename);
            
        }

        /// <summary>
        /// Charge un Automate à partir d'un fichier
        /// </summary>
        /// <param name="filename">chemin du fichier</param>
        /// <returns>Automate chargé</returns>
        public Automate ChargementAutomate(string filename)
        {
            return automateSerializer.ChargementAutomate(filename);
        }

        #endregion
    }
}
