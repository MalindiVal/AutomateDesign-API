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
        private IEtatDAO etatDAO;
        private ITransitionDAO transitionDAO;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="dao">dao dédié aux automates</param>
        public AutomateService(IAutomateDAO dao, IEtatDAO etatDAO, ITransitionDAO transitionDAO)
        {
            this.dao = dao;
            this.etatDAO = etatDAO;
            this.transitionDAO = transitionDAO;
        }

        /// <inheritdoc/>
        public Automate AddAutomate(Automate automate)
        {
            try
            {
                Automate res = this.dao.AddAutomate(automate);
                this.DeduplicateEtats(res);
                etatDAO.InsertEtats(res);
                transitionDAO.InsertTransitions(res);
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
                throw new DAOError("Une erreur s'est produit dans le DAO : " + ex.Message);
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
                throw new DAOError("Une erreur s'est produite lors de la mise à jour de l'automate : " + ex.Message);
            }
        }

        /// <summary>
        /// Supprime les états dupliqués dans un automate en s'assurant que chaque état est unique.
        /// </summary>
        /// <param name="automate">L'automate à vérifier</param>
        private void DeduplicateEtats(Automate automate)
        {
            foreach (var transition in automate.Transitions)
            {
                if (!automate.Etats.Contains(transition.EtatDebut))
                    automate.Etats.Add(transition.EtatDebut);
                if (!automate.Etats.Contains(transition.EtatFinal))
                    automate.Etats.Add(transition.EtatFinal);
            }

            var uniqueEtats = new HashSet<Etat>();
            var etatMap = new Dictionary<Etat, Etat>();

            foreach (var etat in automate.Etats)
            {
                if (uniqueEtats.TryGetValue(etat, out var existing))
                {
                    etatMap[etat] = existing;
                }
                else
                {
                    uniqueEtats.Add(etat);
                    etatMap[etat] = etat;
                }
            }

            foreach (var transition in automate.Transitions)
            {
                if (etatMap.TryGetValue(transition.EtatDebut, out var debut))
                    transition.EtatDebut = debut;
                if (etatMap.TryGetValue(transition.EtatFinal, out var fin))
                    transition.EtatFinal = fin;
            }

            automate.Etats = uniqueEtats.ToList();
        }

    }
}
