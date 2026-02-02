using LogicLayer;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Implementaions
{
    /// <summary>
    /// Implémentation de <see cref="IAutomateSerializer"/> utilisant le format JSON.
    /// </summary>
    public class JsonAutomateSerializer : IAutomateSerializer
    {
        private JsonSerializerOptions options;

        /// <summary>
        /// Constructeur
        /// </summary>
        public JsonAutomateSerializer() 
        {
            options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };
        }

        /// <summary>
        /// Chargement des automate en passant par le DTO.
        /// </summary>
        /// <param name="filename">Nom du fichier à ouvrir.</param>
        /// <returns>L'automate</returns>
        /// <exception cref="InvalidOperationException">Exception levée si le fichier JSON ne contient pas d'automate valide.</exception>
        public Automate ChargementAutomate(string filename)
        {
            string json = File.ReadAllText(filename);

            // Désérialisation dans le DTO
            AutomateDto? dto = JsonSerializer.Deserialize<AutomateDto>(json, options);

            if (dto == null)
                throw new InvalidOperationException("Le fichier JSON ne contient pas d'automate valide.");

            // Création de l'objet métier
            Automate automate = new Automate
            {
                Id = dto.Id,
                Nom = dto.Nom,
                Utilisateur = dto.Utilisateur
            };

            // Hydratation des états
            foreach (Etat e in dto.Etats)
            {
                automate.AjouterEtat(e);
            }

            // Hydratation des transitions
            foreach (Transition t in dto.Transitions)
            {
                automate.AjouterTransition(t);
            }

            return automate;
        }

        /// <summary>
        /// Sauvegarde d'un automate en passant par le DTO.
        /// </summary>
        /// <param name="automate">Automate à sauvegarder.</param>
        /// <param name="filename">Nom du fichier à créer.</param>
        public void SauvegardeAutomate(Automate automate, string filename)
        {
            // Construction du DTO à partir de l'objet métier
            var dto = new AutomateDto
            {
                Id = automate.Id,
                Nom = automate.Nom,
                Utilisateur = automate.Utilisateur,
                Etats = automate.Etats.ToList(),
                Transitions = automate.Transitions.ToList()
            };

            // Sérialisation du DTO
            string json = JsonSerializer.Serialize(dto, options);
            File.WriteAllText(filename, json);
        }
    }
}
