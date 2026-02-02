using LogicLayer;
using Microsoft.Data.Sqlite;
using System.Data;

namespace API.Data
{
    /// <summary>
    /// Connecteur à la bdd
    /// </summary>
    public class SQLiteConnector : IDisposable
    {
        #region Attributs
        //Connection
        private SqliteConnection connection;
        #endregion

        #region Constructeur
        /// <summary>
        /// Création de la connection
        /// </summary>
        public SQLiteConnector()
        {
            BDDConfig config = BDDConfig.Load();
            this.connection = new SqliteConnection(config.Source);
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Exécute une requête
        /// </summary>
        /// <param name="query">Requête</param>
        /// <param name="parameters">Paramètres</param>
        /// <returns>La réponse de la bdd</returns>
        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {

            this.connection.Open();
            DataTable dataTable = new DataTable();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }
            this.connection.Close();
            return dataTable;
        }

        /// <summary>
        /// Execute un insert et renvoie l'id de celui-ci
        /// </summary>
        /// <param name="query">La requête d'insert</param>
        /// <param name="parameters">Le dictionnaire des paramètres</param>
        /// <returns>L'id de la ligne inséré</returns>
        public long ExecuteInsert(string query, Dictionary<string, object> parameters = null)
        {
            long id = -1;

            try
            {
                this.connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = query + "; SELECT last_insert_rowid();";

                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }

                    id = Convert.ToInt64(command.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new DAOError("Erreur SQLite lors de l'INSERT.", ex);
            }
            finally
            {
                this.connection.Close();
            }
            return id;
        }


        /// <summary>
        /// Exécute une requête de type non-SELECT (INSERT/UPDATE/DELETE)
        /// </summary>
        /// <param name="query">Requête SQL</param>
        /// <param name="parameters">Paramètres</param>
        /// <returns>Nombre de lignes affectées</returns>
        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            this.connection.Open();
            int affectedRows = 0;

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                affectedRows = command.ExecuteNonQuery();
            }

            this.connection.Close();
            return affectedRows;
        }


        /// <summary>
        /// Fermeture de la connexion
        /// </summary>
        public void Dispose()
        {
            if (connection != null)
            {
                if (connection.State != System.Data.ConnectionState.Closed) connection.Close();
                connection.Dispose();
                connection = null;
            }
        }
        #endregion
    }
}
