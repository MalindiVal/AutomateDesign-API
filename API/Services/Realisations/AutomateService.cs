using API.Data.Interfaces;
using API.Services.Interfaces;
using LogicLayer;
using System.Collections.Generic;

namespace API.Services.Realisations
{
    /// <summary>
    /// Implémentation concrète du service métier pour la gestion des entités <see cref="Automate"/>.
    /// Cette classe fait le lien entre la couche API et la couche DAO.
    /// </summary>
    public class AutomateService : IAutomateService
    {
        private IAutomateDAO dao;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="dao">dao dédié aux automates</param>
        public AutomateService(IAutomateDAO dao)
        {
            this.dao = dao;
        }

        /// <inheritdoc/>
        public Automate AddAutomate(Automate automate)
        {
            try
            {
                Automate res = this.dao.AddAutomate(automate);
                return res;
            }
            catch (Exception ex)
            {
                throw new DAOError("Une erreur s'est produit dans le DAO",ex);
            }
        }

        /// <inheritdoc/>
        public List<Automate> GetAllAutomates()
        {
            try
            {
                List <Automate> res = new List<Automate>();
                res = this.dao.GetAllAutomates();
                return res;
            }
            catch (Exception ex)
            {
                throw new DAOError("Une erreur s'est produit dans le DAO");
            }
        }

        /// <inheritdoc/>
        public List<Automate> GetAllAutomatesByUser(Utilisateur user)
        {
            try
            {
                List<Automate> res = new List<Automate>();
                res = this.dao.GetAllAutomatesByUser(user);
                return res;
            }
            catch (Exception ex)
            {
                throw new DAOError("Une erreur s'est produit dans le DAO",ex);
            }
        }

        /// <inheritdoc/>
        public Automate GetAutomate(int id)
        {
            try
            {
                Automate res = new Automate();
                res = this.dao.GetAutomate(id);
                return res;
            }
            catch (Exception ex)
            {
                throw new DAOError("Une erreur s'est produit dans le DAO",ex);
            }
        }

        /// <inheritdoc/>
        public Automate UpdateAutomate(Automate automate)
        {
            try
            {
                Automate res = this.dao.AddAutomate(automate);
                return res;
            }
            catch (Exception ex)
            {
                throw new DAOError("Une erreur s'est produite lors de la mise à jour de l'automate.");
            }
        }

    }
}
