using System.Net.Http.Json;
using System.Text.Json;


namespace Client.Data
{
    /// <summary>
    /// DAO Générique
    /// </summary>
    public abstract class DAO
    {
        #region Attributs
        //Le client Http
        private HttpClient client;

        //Adresse de l'API
        private string adressAPI;

        /// <summary>
        /// Option pour le sérialiseur
        /// </summary>
        private JsonSerializerOptions options = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        };
        #endregion

        #region Propriétés
        /// <summary>
        /// Options du sérialiseur JSON
        /// </summary>
        public JsonSerializerOptions Options
        {
            get { return options; }
        }
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur
        /// </summary>
        public DAO()
        {
            APIConfig config = APIConfig.Load();

            if (config.LocalMode)
            {
                adressAPI = config.LocalUrl;
                client = new HttpClient();
            }
            else
            {
                adressAPI = config.BaseUrl;
                string certificat = config.APICertificate;

                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => {
                    bool res = false;
                    if (cert.GetCertHashString() == certificat.Replace(":", ""))
                    {
                        res = true;
                    }
                    return res;
                };
                client = new HttpClient(handler);
            }
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Récupère de façon assynchrone
        /// </summary>
        /// <param name="demande">Fin de l'URL</param>
        /// <returns>La réponse http</returns>
        public async Task<HttpResponseMessage> GetAsync(string demande)
        {
            string adresseEnvoi = adressAPI + demande;
            return await client.GetAsync(adresseEnvoi);
        }

        /// <summary>
        /// Envoi de façon assynchrone
        /// </summary>
        /// <param name="demande">Fin de l'URL</param>
        /// <param name="objet">l'objet à envoyer</param>
        /// <returns>La réponse http</returns>
        public async Task<HttpResponseMessage> PostAsync(string demande, Object objet)
        {
            string adresseEnvoi = adressAPI + demande;
            return await client.PostAsJsonAsync(adresseEnvoi, objet);
        }

        /// <summary>
        /// Mise à jour de façon asynchrone (PUT - modification)
        /// </summary>
        /// <param name="demande">Fin de l'URL</param>
        /// <param name="objet">L'objet à mettre à jour</param>
        /// <returns>La réponse http</returns>
        public async Task<HttpResponseMessage> PutAsync(string demande, object objet)
        {
            string adresseEnvoi = adressAPI + demande;
            return await client.PutAsJsonAsync(adresseEnvoi, objet);
        }
        #endregion
    }
}
