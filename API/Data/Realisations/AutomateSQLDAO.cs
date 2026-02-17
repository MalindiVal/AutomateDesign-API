using API.Data.Interfaces;
using LogicLayer;
using System.Data;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace API.Data.Realisations
{
    /// <summary>
    /// Implémentation concrète du DAO pour la gestion des entités <see cref="Automate"/>.
    /// Cette classe gère les opérations d'accès à la base de données SQLite,
    /// notamment la lecture, l'insertion et la reconstruction complète des automates,
    /// avec leurs états et transitions.
    /// </summary>
    public class AutomateSQLDAO : IAutomateDAO
    {
        private readonly IBDDConnection connection;

        public AutomateSQLDAO(IBDDConnection connection)
        {
            this.connection = connection;
        }

        /// <inheritdoc/>
        #region Méthodes publiques
        public Automate AddAutomate(Automate automate)
        {
            if (automate.Id != null)
            {
                this.UpdateAutomate(automate);
            }
            else
            {
                this.CreateAutomate(automate);
            }
            return automate;
        }

        public void DeleteAutomate(int id, int idUser)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", id },
                     { "@IdUser", idUser }
                };

            connection.ExecuteNonQuery(
                "DELETE FROM Automates WHERE Id = @Id AND IdUser = @IdUser", parameters
            );

            connection.ExecuteNonQuery("DELETE FROM Transitions WHERE IdAutomate = @Id", parameters);
            connection.ExecuteNonQuery("DELETE FROM Etats WHERE IdAutomate = @Id", parameters);

        }

        /// <summary>
        /// Crée un nouvel automate et lui attribue un Id.
        /// </summary>
        /// <param name="automate"></param>
        private void CreateAutomate(Automate automate)
        {

            Utilisateur createur = automate.Utilisateur;
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Nom", automate.Nom },
                    { "@Id", createur.Id }
                };

            automate.Id = (int)connection.ExecuteInsert(
                "INSERT INTO Automates (Nom,IdUser) VALUES (@Nom,@Id)", parameters
            );
        }

        /// <summary>
        /// Met à jour un automate existant et supprime ses états et transitions pour les recréer.
        /// </summary>
        /// <param name="automate"></param>
        private void UpdateAutomate(Automate automate)
        {
            Utilisateur createur = automate.Utilisateur;
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", automate.Id },
                    { "@Nom", automate.Nom },
                     { "@IdUser", createur.Id }
                };

            DataTable res = connection.ExecuteQuery("Select Id From Automates Where Id = @Id AND IdUser = @IdUser", parameters);

            if (res.Rows.Count > 0)
            {

                connection.ExecuteNonQuery(
                    "UPDATE Automates SET Nom = @Nom WHERE Id = @Id AND IdUser = @IdUser", parameters
                );


                connection.ExecuteNonQuery("DELETE FROM Transitions WHERE IdAutomate = @Id", parameters);
                connection.ExecuteNonQuery("DELETE FROM Etats WHERE IdAutomate = @Id", parameters);
            }
        }

        /// <inheritdoc/>
        public List<Automate> GetAllAutomates()
        {
            List<Automate> result = new List<Automate>();

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

            return result;
        }

        /// <inheritdoc/>
        public List<Automate> GetAllAutomatesByUser(Utilisateur user)
        {
            List<Automate> result = new List<Automate>();

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

            return result;
        }

        /// <inheritdoc/>
        public Automate GetAutomate(int id)
        {
            Automate? result = null;

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

                Dictionary<int, Etat> etatDictionary = this.GetEtatsFromRows(result);
                this.GetTransitionsFromRows(result, etatDictionary);
            }

            if (result == null)
                throw new Exception($"Automate avec Id {id} non trouvé.");

            return result;
        }
        #endregion

        /// <summary>
        /// Récupère les états liés à un automate à partir des lignes de la base de données.
        /// </summary>
        /// <param name="automate">Automate avec ses propres informations mais sans ses etats</param>
        /// <returns>Un dictionnaire des états de l'automate</returns>
        private Dictionary<int, Etat> GetEtatsFromRows(Automate automate)
        {
            Dictionary<int, Etat> result = new Dictionary<int, Etat>();

            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", automate.Id } };
            string query = "SELECT Nom, Id, X, Y, estInitial, estFinal FROM Etats WHERE IdAutomate = @Id";
            DataTable etats = connection.ExecuteQuery(query, parameters);

            foreach (DataRow r in etats.Rows)
            {
                Etat e = new Etat
                {
                    Id = Convert.ToInt32(r["Id"]),
                    Nom = r["Nom"].ToString(),
                    Position = new Position(Convert.ToDouble(r["X"]), Convert.ToDouble(r["Y"])),
                    EstInitial = Convert.ToInt32(r["estInitial"]) == 1,
                    EstFinal = Convert.ToInt32(r["estFinal"]) == 1
                };

                result[e.Id] = e;
                automate.Etats.Add(e);
            }

            return result;
        }

        /// <summary>
        /// Récupère les transitions liées à un automate à partir des lignes de la base de données.
        /// </summary>
        /// <param name="automate">Automate avec ses propres informations mais sans ses transitions</param>
        /// <param name="etatDictionary">Dictionnaire d'états</param>
        private void GetTransitionsFromRows(Automate automate, Dictionary<int, Etat> etatDictionary)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", automate.Id } };
            string query = "SELECT Condition, EtatDebut, EtatFinal, IdAutomate , X , Y FROM Transitions WHERE IdAutomate = @Id";
            DataTable transitions = connection.ExecuteQuery(query, parameters);

            foreach (DataRow res in transitions.Rows)
            {
                int idEtat1 = Convert.ToInt32(res["EtatDebut"]);
                int idEtat2 = Convert.ToInt32(res["EtatFinal"]);

                if (etatDictionary.ContainsKey(idEtat1) && etatDictionary.ContainsKey(idEtat2))
                {
                    Transition t = new Transition(etatDictionary[idEtat1], etatDictionary[idEtat2])
                    {
                        Condition = res["Condition"].ToString(),
                        ManualControlX = res["X"] != DBNull.Value ? Convert.ToDouble(res["X"]) : (double?)null,
                        ManualControlY = res["Y"] != DBNull.Value ? Convert.ToDouble(res["Y"]) : (double?)null
                    };

                    automate.Transitions.Add(t);
                }
            }
        }

    }

}
