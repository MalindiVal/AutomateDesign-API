using API.Data.Interfaces;
using LogicLayer;
using System.Data;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using API.Data.Realisations.Writers;
using API.Data.Realisations.Readers;
using API.Data.Realisations.Extensions;

namespace API.Data.Realisations
{
    /// <summary>
    /// Implémentation concrète du DAO pour la gestion des entités <see cref="Automate"/>.
    /// Cette classe gère les opérations d'accès à la base de données SQLite,
    /// notamment la lecture, l'insertion et la reconstruction complète des automates,
    /// avec leurs états et transitions.
    /// </summary>
    public class AutomateDAO : IAutomateDAO
    {
        /// <inheritdoc/>
        #region Méthodes publiques
        public Automate AddAutomate(Automate automate)
        {
            if (automate.Id != null)
            {
                    AutomateDbWriter.UpdateAutomate(automate);
            }
            else
            {
                AutomateDbWriter.CreateAutomate(automate);
            }
            AutomateExtensions.DeduplicateEtats(automate);
            AutomateDbWriter.InsertEtats(automate);
            AutomateDbWriter.InsertTransitions(automate);
            return automate;
        }

        /// <inheritdoc/>
        public List<Automate> GetAllAutomates()
        {
            List<Automate> result = new List<Automate>();

            using (SQLiteConnector connection = new SQLiteConnector())
            {
                string query = "SELECT Id, Nom FROM Automates";
                DataTable data = connection.ExecuteQuery(query);

                foreach (DataRow row in data.Rows)
                {
                    Automate a = new Automate
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Nom = row["Nom"].ToString()
                    };

                    result.Add(a);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public List<Automate> GetAllAutomatesByUser(Utilisateur user)
        {
            List<Automate> result = new List<Automate>();

            using (SQLiteConnector connection = new SQLiteConnector())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                    {
                        {"@Id",user.Id }
                    };
                string query = "SELECT Id, Nom FROM Automates Where idUser = @Id";
                DataTable data = connection.ExecuteQuery(query, parameters);

                foreach (DataRow row in data.Rows)
                {
                    Automate a = new Automate
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Nom = row["Nom"].ToString()
                    };

                    result.Add(a);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public Automate GetAutomate(int id)
        {
            Automate? result = null;

            using (SQLiteConnector connection = new SQLiteConnector())
            {
                string query = "SELECT Id, Nom FROM Automates WHERE Id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable data = connection.ExecuteQuery(query, parameters);

                if (data.Rows.Count > 0)
                {
                    result = new Automate
                    {
                        Id = id,
                        Nom = data.Rows[0]["Nom"].ToString()
                    };

                    Dictionary<int, Etat> etatDictionary = AutomateDbReader.GetEtatsFromRows(result);
                    AutomateDbReader.GetTransitionsFromRows(result, etatDictionary);
                }
            }

            if (result == null)
                throw new Exception($"Automate avec Id {id} non trouvé.");

            return result;
        }
        #endregion
    }
}
