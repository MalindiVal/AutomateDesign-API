using System.Text.Json;
using System.IO;

namespace API.Data
{
    /// <summary>
    /// Représente la configuration de la base de données, chargée à partir d’un fichier JSON.
    /// </summary>
    public class BDDConfig
    {
        private string source;

        /// <summary>
        /// Chaîne de connexion à la source de données de la base.
        /// </summary>
        public string Source
        {
            get
            {
                return source;
            }
        }

        /// <summary>
        /// Charge la configuration de la base de données à partir d’un fichier JSON spécifié.
        /// </summary>
        /// <param name="filePath">
        /// Le chemin absolu ou relatif vers le fichier de configuration. 
        /// Par défaut, ce chemin est "appsettings.json".
        /// </param>
        /// <returns>
        /// Retourne un objet BDDConfig contenant la chaîne de connexion initialisée.
        /// </returns>
        public static BDDConfig Load(string filePath = "appsettings.json")
        {
            // Lecture du contenu du fichier JSON de configuration
            string json = File.ReadAllText(filePath);

            // Analyse du document JSON
            using var doc = JsonDocument.Parse(json);

            // Accès à la section "BDD" du fichier de configuration
            var bdd = doc.RootElement.GetProperty("BDD");

            // Extraction du chemin de la base de données et création de la configuration
            return new BDDConfig
            {
                source = "Data Source =" + bdd.GetProperty("FilePath").GetString(),
            };
        }
    }
}
