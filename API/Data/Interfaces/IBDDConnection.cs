using System.Data;

namespace API.Data.Interfaces
{
    /// <summary>
    /// Interface pour la gestion des connexions et opérations de base de données.
    /// Fournit des méthodes pour exécuter des requêtes SQL, des inserts et des commandes non-SELECT.
    /// </summary>
    public interface IBDDConnection
    {
        /// <summary>
        /// Exécute une requête
        /// </summary>
        /// <param name="query">Requête</param>
        /// <param name="parameters">Paramètres</param>
        /// <returns>La réponse de la bdd</returns>
        DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null);

        /// <summary>
        /// Execute un insert et renvoie l'id de celui-ci
        /// </summary>
        /// <param name="query">La requête d'insert</param>
        /// <param name="parameters">Le dictionnaire des paramètres</param>
        /// <returns>L'id de la ligne inséré</returns>
        long ExecuteInsert(string query, Dictionary<string, object> parameters = null);

        /// <summary>
        /// Exécute une requête de type non-SELECT (INSERT/UPDATE/DELETE)
        /// </summary>
        /// <param name="query">Requête SQL</param>
        /// <param name="parameters">Paramètres</param>
        /// <returns>Nombre de lignes affectées</returns>
        int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null);

    }
}
