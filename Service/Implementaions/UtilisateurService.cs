using ClientData.Interfaces;
using LogicLayer;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementaions
{
    /// <summary>
    /// Implémentation de l'interface IUtilisateurService qui utilise UtilisateurDAO
    /// </summary>
    public class UtilisateurService : IUtilisateurService
    {
        private IUtilisateurDAO dao;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="dao">dao</param>
        public UtilisateurService(IUtilisateurDAO dao)
        {
            this.dao = dao;
        }

        /// <inheritdoc>
        public async Task<Utilisateur> Login(Utilisateur user)
        {
            return await this.dao.Login(user);
        }

        /// <inheritdoc>
        public async Task<Utilisateur> Register(Utilisateur user)
        {
            return await this.dao.Register(user);
        }
    }
}
