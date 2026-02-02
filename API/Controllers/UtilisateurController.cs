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
        private readonly ITokenService tokenService;
        #endregion

        #region Constructeur
        /// <summary>
        /// Initialise une nouvelle instance du contrôleur <see cref="UtilisateurController"/>.
        /// </summary>
        /// <param name="service">Service applicatif chargé de la logique métier des utilisateurs.</param>
        /// <param name="tokenService">Service pour la génération de token</param>
        public UtilisateurController(IUtilisateurService service, ITokenService tokenService)
        {
            this.service = service;
            this.tokenService = tokenService;
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Permet de faire une tentative de connexion
        /// </summary>
        /// <param name="login">les données utilisateur avec le login et le mot de passe</param>
        /// <returns>Utilisateur avec l'id</returns>
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Utilisateur login)
        {
            IActionResult res = BadRequest();
            if (login == null || string.IsNullOrWhiteSpace(login.Login) || string.IsNullOrWhiteSpace(login.Mdp))
            {
                res = BadRequest("Login ou mot de passe manquant");
            } else
            {
                try
                {

                    Utilisateur user = service.Login(login);


                    if (user?.Id != null)
                    {
                        var token = tokenService.GenerateToken(user);

                        res = Ok(new
                        {
                            token,
                            user = new
                            {
                                user.Id,
                                user.Login,
                            }
                        });
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
            }
                
            return res;
        }

        /// <summary>
        /// Permet d'enregistrer un utilisateur
        /// </summary>
        /// <param name="user">utilisateur à enregistrer</param>
        /// <returns>Utilisateur avec l'id</returns>
        [HttpPost("Register")]
        public IActionResult Register([FromBody]Utilisateur user)
        {
            IActionResult res = BadRequest();
            try
            {
                Utilisateur result = service.Register(user);
                if (result.Id != null)
                {
                    var token = tokenService.GenerateToken(result);

                    res = Ok(new
                    {
                        token,
                        user = new
                        {
                            result.Id,
                            result.Login,
                        }
                    });
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
