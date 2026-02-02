using Client.Data;
using ClientData.Interfaces;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientData.Realisations
{
    /// <summary>
    /// Implémentation concrète de <see cref="IUtilisateurDAO"/> permettant la gestion des utilisateurs via l’API distante.
    /// </summary>
    public class UtilisateurDAO : DAO, IUtilisateurDAO
    {
        /// inheritdoc/>
        public async Task<Utilisateur> Login(Utilisateur user)
        {
            Utilisateur res = new Utilisateur();
            HttpResponseMessage reponseHttp = await this.PostAsync("Utilisateur/Login", user);
            if (reponseHttp.IsSuccessStatusCode)
            {
                string reponse = await reponseHttp.Content.ReadAsStringAsync();
                res = JsonSerializer.Deserialize<Utilisateur>(reponse, Options);
            }
            return res;
        }

        /// inheritdoc/>
        public async Task<Utilisateur> Register(Utilisateur user)
        {
            Utilisateur res = new Utilisateur();
            HttpResponseMessage reponseHttp = await this.PostAsync("Utilisateur/Register", user);
            if (reponseHttp.IsSuccessStatusCode)
            {
                string reponse = await reponseHttp.Content.ReadAsStringAsync();
                res = JsonSerializer.Deserialize<Utilisateur>(reponse, Options);
            }
            return res;
        }
    }
}
