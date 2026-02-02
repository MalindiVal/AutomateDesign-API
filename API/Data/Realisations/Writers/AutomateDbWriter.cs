using LogicLayer;
using System.Data;

namespace API.Data.Realisations.Writers
{
    /// <summary>
    /// Fournit des méthodes pour écrire des automates dans la base de données.
    /// </summary>
    internal static class AutomateDbWriter
    {
        /// <summary>
        /// Crée un nouvel automate et lui attribue un Id.
        /// </summary>
        /// <param name="automate"></param>
        internal static void CreateAutomate(Automate automate)
        {
            using (SQLiteConnector connection = new SQLiteConnector())
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
        }

        /// <summary>
        /// Met à jour un automate existant et supprime ses états et transitions pour les recréer.
        /// </summary>
        /// <param name="automate"></param>
        internal static void UpdateAutomate(Automate automate)
        {
            using (SQLiteConnector connection = new SQLiteConnector())
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
        }

        internal static void DeleteAutomate(Automate automate)
        {
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                Utilisateur createur = automate.Utilisateur;
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", automate.Id },
                    { "@Nom", automate.Nom },
                     { "@IdUser", createur.Id }
                };

                DataTable res = connection.ExecuteQuery("Select Id From Automates Where Id = @Id AND IdUser = @IdUser", parameters);

                if (res != null)
                {
                    connection.ExecuteNonQuery(
                        "DELETE FROM Automates WHERE Id = @Id AND IdUser = @IdUser", parameters
                    );


                    connection.ExecuteNonQuery("DELETE FROM Transitions WHERE IdAutomate = @Id", parameters);
                    connection.ExecuteNonQuery("DELETE FROM Etats WHERE IdAutomate = @Id", parameters);
                }
                
            }
        }

        /// <summary>
        /// Insère les états liés à l'automate.
        /// </summary>
        /// <param name="automate"></param>
        internal static void InsertEtats(Automate automate)
        {
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                foreach (Etat p in automate.Etats)
                {
                    int final = p.EstFinal ? 1 : 0;
                    int init = p.EstInitial ? 1 : 0;
                    Dictionary<string, object> parameters = new Dictionary<string, object>()
                    {
                        {"@Id",automate.Id },
                        {"@X",p.Position.X },
                        {"@Y",p.Position.Y },
                        {"@Final",final },
                        {"@Initial",init },
                        {"@NomEtat",p.Nom }
                    };

                    p.Id = (int)connection.ExecuteInsert(
                        "INSERT INTO Etats (Nom,X,Y,IdAutomate,estInitial,estFinal) VALUES (@NomEtat,@X,@Y,@Id,@Initial,@Final)",
                        parameters
                    );
                }
            }
        }

        /// <summary>
        /// Insère les transitions liées à l'automate.
        /// </summary>
        /// <param name="automate"></param>
        /// <exception cref="DAOError"></exception>
        internal static void InsertTransitions(Automate automate)
        {
            using (SQLiteConnector connection = new SQLiteConnector())
            {

                foreach (Transition p in automate.Transitions)
                {
                    if (p.EtatDebut.Id <= 0 || p.EtatFinal.Id <= 0)
                    {
                        throw new DAOError("Les états des transitions doivent avoir des Ids valides");
                    }
                    
                    Dictionary<string, object> parameters = new Dictionary<string, object>()
                    {
                        { "@Id", automate.Id },
                        { "@NomTransition", p.Condition },
                        { "@Debut", p.EtatDebut.Id },
                        { "@Final", p.EtatFinal.Id },
                        { "@X", p.ManualControlX.HasValue ? (object)p.ManualControlX.Value : DBNull.Value },
                        { "@Y", p.ManualControlY.HasValue ? (object)p.ManualControlY.Value : DBNull.Value },
                    };

                    string query = "INSERT INTO Transitions (Condition, EtatDebut, EtatFinal, IdAutomate,X,Y) " +
                                   "VALUES (@NomTransition, @Debut, @Final, @Id,@X,@Y)";

                    int v = (int)connection.ExecuteInsert(query, parameters);

                    if (v == -1)
                    {
                        throw new DAOError("Erreur lors de l'insertion d'une transition");
                    }
                }
            }
        }
    }
}
