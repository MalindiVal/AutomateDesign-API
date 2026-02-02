using API.Services.Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Contrôleur d'API responsable de la gestion des entités <see cref="Utilisateur"/>.
    /// </summary>
    [ApiController]
    [Route("Utilisateur")]
    public class UtilisateurController : Controller
    {
        #region Attributs
        private readonly IUtilisateurService service;
        #endregion

        #region Constructeur
        /// <summary>
        /// Initialise une nouvelle instance du contrôleur <see cref="UtilisateurController"/>.
        /// </summary>
        /// <param name="service">Service applicatif chargé de la logique métier des utilisateurs.</param>
        public UtilisateurController(IUtilisateurService service)
        {
            this.service = service;
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Permet de faire une tentative de connexion
        /// </summary>
        /// <param name="login">les données utilisateur avec le login et le mot de passe</param>
        /// <returns>Utilisateur avec l'id</returns>
        [HttpPost("Login")]
        public ActionResult<Utilisateur> Login([FromBody] Utilisateur login)
        {
            ActionResult<Utilisateur> res = BadRequest();
            try
            {
                
                Utilisateur user = service.Login(login);

                
                if (user?.Id != null)
                {
                    res = Ok(user);
                }
                else
                {
                    res = Unauthorized("Identifiants incorrects.");
                }
            }
            catch (Exception ex)
            {
                res = StatusCode(500, $"Une erreur interne est survenue lors de la connexion : {ex.Message}");
            }
            return res;
        }

        /// <summary>
        /// Permet d'enregistrer un utilisateur
        /// </summary>
        /// <param name="user">utilisateur à enregistrer</param>
        /// <returns>Utilisateur avec l'id</returns>
        [HttpPost("Register")]
        public ActionResult<Utilisateur> Register([FromBody]Utilisateur user)
        {
            ActionResult<Utilisateur> res = BadRequest();
            try
            {
                Utilisateur result = service.Register(user);
                if (result.Id != null)
                {
                    res = Ok(result);
                }
            }
            catch (Exception ex)
            {
                res = StatusCode(500, $"Une erreur interne est survenue lors de l'enregistrement d'un utilisateur : {ex.Message}");
            }
            return res;
        }
        #endregion
    }
}
