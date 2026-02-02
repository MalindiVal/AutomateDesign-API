using Client.Data;
using ClientData.Interfaces;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientData.Realisations
{
    /// <summary>
    /// Implémentation concrète de <see cref="IAutomateDAO"/> permettant la gestion des automates via l’API distante.
    /// </summary>
    public class AutomateDAO : DAO, IAutomateDAO
    {
        /// <inheritdoc/>
        public async Task<Automate> AddAutomate(Automate automate)
        {
            if(automate.Utilisateur == null)
            {
                throw new DAOError("Veuillez vous connecter avant de réaliser une exportation en ligne");
            }

            // Conversion de l'automate en DTO pour l'envoi
            AutomateDto dtoToSend = AutomateDto.FromDomain(automate);

            HttpResponseMessage reponseHttp = await this.PostAsync("Automate/ExportAutomate", dtoToSend);
            if (reponseHttp.IsSuccessStatusCode)
            {
                string reponse = await reponseHttp.Content.ReadAsStringAsync();
                AutomateDto? dtoBack = JsonSerializer.Deserialize<AutomateDto>(reponse, Options);

                if (dtoBack == null)
                    throw new DAOError("Réponse invalide lors de l'ajout de l'automate.");
                return dtoBack.ToDomain();
            } else
            {
                throw new DAOError("Le nom " + automate.Nom + " existe déja");
            }
        }

        /// <inheritdoc/>
        public async Task<List<Automate>> GetAllAutomates()
        {
            List<Automate> list = new List<Automate>();
            HttpResponseMessage reponseHttp = await this.GetAsync("Automate/GetAllAutomates");

            if (reponseHttp.IsSuccessStatusCode)
            {
                string reponse = await reponseHttp.Content.ReadAsStringAsync();
                List<AutomateDto>? dtoList = JsonSerializer.Deserialize<List<AutomateDto>>(reponse, Options) ?? new List<AutomateDto>();

                list = dtoList.Select(d => d.ToDomain()).ToList();
            }

            return list;
        }

        /// <inheritdoc/>
        public async Task<List<Automate>> GetAllAutomatesByUser(Utilisateur user)
        {
            List<Automate> list = new List<Automate>();
            HttpResponseMessage reponseHttp = await this.PostAsync("Automate/GetAllAutomatesByUser",user);

            if (reponseHttp.IsSuccessStatusCode)
            {
                string reponse = await reponseHttp.Content.ReadAsStringAsync();
                List<AutomateDto>? dtoList = JsonSerializer.Deserialize<List<AutomateDto>>(reponse, Options) ?? new List<AutomateDto>();

                list = dtoList.Select(d => d.ToDomain()).ToList();
            }

            return list;
        }

        /// <inheritdoc/>
        public async Task<Automate> GetAutomate(int id)
        {
            Automate a = new Automate();
            a.Id = id;
            HttpResponseMessage reponseHttp = await this.GetAsync("Automate/GetAutomateById?id=" + id);
            if (reponseHttp.IsSuccessStatusCode)
            {
                string reponse = await reponseHttp.Content.ReadAsStringAsync();
                AutomateDto? dto = JsonSerializer.Deserialize<AutomateDto>(reponse, Options);

                if (dto != null)
                    a = dto.ToDomain();
            }

            return a;
        }

        /// <inheritdoc/>
        public async Task<Automate> UpdateAutomate(Automate automate)
        {
            if (!automate.Id.HasValue)
            {
                throw new DAOError("L'automate doit avoir un ID pour être mis à jour");
            }

            if (automate.Utilisateur == null)
            {
                throw new DAOError("L'automate doit avoir un ID pour être mis à jour");
            }

            AutomateDto dtoToSend = AutomateDto.FromDomain(automate);

            HttpResponseMessage response = await this.PutAsync($"Automate/UpdateAutomate", dtoToSend);

            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                AutomateDto? dtoBack = JsonSerializer.Deserialize<AutomateDto>(responseJson, Options);

                if (dtoBack == null)
                    throw new DAOError("Réponse invalide lors de la mise à jour de l'automate.");

                return dtoBack.ToDomain();
            }
            else
            {
                throw new DAOError($"Erreur lors de la mise à jour : {response.StatusCode}");
            }
        }   
    }
}
