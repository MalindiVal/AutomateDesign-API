using API.Services.Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Contrôleur d'API responsable de la gestion des entités <see cref="Automate"/>.
    /// Permet la récupération et l'exportation d'automates via les services applicatifs.
    /// </summary>
    [ApiController]
    [Route("Automate")]
    public class AutomateController : Controller
    {
        #region Attributs
        private readonly IAutomateService service;
        #endregion

        #region Constructeur
        /// <summary>
        /// Initialise une nouvelle instance du contrôleur <see cref="AutomateController"/>.
        /// </summary>
        /// <param name="service">Service applicatif chargé de la logique métier des automates.</param>
        public AutomateController(IAutomateService service)
        {
            this.service = service;
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Récupère la liste de tous les automates.
        /// </summary>
        /// <returns>Liste des automates ou code d'erreur.</returns>
        [Authorize]
        [HttpGet("GetAllAutomates")]
        public IActionResult GetAllAutomates()
        {
            IActionResult res = BadRequest();
            try
            {
                string? login = User.FindFirstValue(ClaimTypes.Name);
                int id;
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id))
                {
                    res = Unauthorized("Id invalide dans le token");
                }
                else
                {
                    List<Automate> automates = service.GetAllAutomates();
                    res = Ok(automates);
                }
                
            }
            catch (Exception ex)
            {
                res = StatusCode(500, $"Une erreur interne est survenue lors de la récupération de la liste des automates : {ex.Message}");
            }
            return res;
        }

        /// <summary>
        /// Récupère la liste de tous les automates d'un utilisateur spécifiques.
        /// </summary>
        /// <returns>Liste des automates ou code d'erreur.</returns>
        [Authorize]
        [HttpGet("GetAllAutomatesByUser")]
        public IActionResult GetAllAutomatesByUser()
        {
            IActionResult res = BadRequest();
            try
            {
                string? login = User.FindFirstValue(ClaimTypes.Name);
                int id;
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id))
                {
                    res = Unauthorized("Id invalide dans le token");
                }
                else
                {
                    Utilisateur user  = new Utilisateur
                    {
                        Id = id,
                        Login = login
                    };
                    List<Automate> automates = service.GetAllAutomatesByUser(user);
                    res = Ok(automates);
                }
                
            }
            catch (Exception ex)
            {
                res = StatusCode(500, $"Une erreur interne est survenue lors de la récupération de la liste des automates : {ex.Message}");
            }
            return res;
        }

        /// <summary>
        /// Exportation d'un automate et retourne la version complète avec son Id.
        /// </summary>
        /// <param name="automate">Automate à exporter.</param>
        /// <returns>Automate créé ou code d'erreur.</returns>
        [Authorize]
        [HttpPost("ExportAutomate")]
        public IActionResult ExportAutomate([FromBody] Automate automate)
        {
            IActionResult res;
            if (automate == null)
                res = BadRequest("Les données de l’automate sont invalides.");
            else
            {
                try
                {
                    string? login = User.FindFirstValue(ClaimTypes.Name);
                    int id;
                    if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id))
                    {
                        res = Unauthorized("Id invalide dans le token");
                    } else
                    {
                        automate.Utilisateur = new Utilisateur
                        {
                            Id = id,
                            Login = login
                        };
                        Automate created = service.AddAutomate(automate);
                        res = CreatedAtAction(nameof(GetAutomateById), new { id = created.Id }, created);
                    }
                }
                catch (Exception ex)
                {
                    res = StatusCode(500, $"Une erreur interne est survenue lors de l'exportation d'un automate : {ex.Message}");
                }
            }
            return res;
        }

        /// <summary>
        /// Récupération d'un automate en fonction de son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'automate.</param>
        /// <returns>Automate trouvé ou code d'erreur.</returns>
        [HttpGet("GetAutomateById")]
        public IActionResult GetAutomateById(int id)
        {
            IActionResult res = BadRequest();
            try
            {
                Automate? automate = service.GetAutomate(id);
                if (automate == null)
                    res = NotFound($"Aucun automate trouvé avec l'ID {id}");
                else
                {
                    res = Ok(automate);
                }  
            }
            catch (Exception ex)
            {
                res = StatusCode(500, $"Une erreur interne est survenue lors de la récupération d'un automate : {ex.Message}");
            }
            return res;
        }

        /// <summary>
        /// Met à jour un automate existant.
        /// </summary>
        /// <param name="automate">Automate avec les nouvelles données.</param>
        /// <returns>Automate mis à jour ou code d'erreur.</returns>
        [HttpPut("UpdateAutomate")]
        public IActionResult UpdateAutomate([FromBody] Automate automate)
        {
            IActionResult res;

            if (automate == null)
            {
                res = BadRequest("Les données de l'automate sont invalides.");
            } else {
                try
                {
                    string? login = User.FindFirstValue(ClaimTypes.Name);
                    int id;
                    if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id))
                    {
                        res = Unauthorized("Id invalide dans le token");
                    }
                    else
                    {
                        automate.Utilisateur = new Utilisateur
                        {
                            Id = id,
                            Login = login
                        };
                        Automate updated = service.UpdateAutomate(automate);
                        res = Ok(updated);
                    }
                    
                }
                catch (Exception ex)
                {
                    res = StatusCode(500, $"Une erreur interne est survenue de la modification d'un automate : {ex.Message}");
                }
            }

            return res;
        }
        #endregion
    }
}
