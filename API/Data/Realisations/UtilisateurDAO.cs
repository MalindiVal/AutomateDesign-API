using API.Data.Interfaces;
using LogicLayer;
using System.Data;

namespace API.Data.Realisations
{

    /// <summary>
    /// Implémentation concrète du DAO pour la gestion des entités <see cref="Utilisateur"/>.
    /// Cette classe gère les opérations d'accès à la base de données SQLite
    /// </summary>
    public class UtilisateurDAO : IUtilisateurDAO
    {
        /// <inheritdoc/>
        public Utilisateur? GetUserByLogin(string login)
        {
            Utilisateur result = null;
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                string query = "Select u.Id , u.Login, p.Hash FROM Utilisateurs u JOIN Passwords p ON u.id = p.IdUser WHERE u.Login = @Login";

                var parameters = new Dictionary<string, object>
                {
                    { "@Login", login }
                };

                var data = connection.ExecuteQuery(query, parameters);

                if (data.Rows.Count > 0)
                {
                    result = new Utilisateur
                    {
                        Id = Convert.ToInt32(data.Rows[0]["Id"]),
                        Login = data.Rows[0]["Login"].ToString(),
                        Mdp = data.Rows[0]["Hash"].ToString()
                    };
                }
            }
            return result;
        }

        /// <inheritdoc/>
        public Utilisateur Register(Utilisateur user)
        {
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                string query = "INSERT INTO Utilisateurs (Login) Values (@Login)";

                var parameters = new Dictionary<string, object>
                {
                    { "@Login", user.Login }
                };

                user.Id =(int)connection.ExecuteInsert(query, parameters);

                query = "INSERT INTO Passwords (IdUser,Hash) Values (@Id,@Hash)";

                parameters = new Dictionary<string, object>
                {
                    { "@Id", user.Id },
                    { "@Hash", user.Mdp },
                };

                int res = (int)connection.ExecuteInsert(query, parameters);

            }
            return user;
        }
    }
}
