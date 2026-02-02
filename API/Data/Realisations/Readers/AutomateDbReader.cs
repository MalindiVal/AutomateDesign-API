using LogicLayer;
using System.Data;

namespace API.Data.Realisations.Readers
{
    /// <summary>
    /// Lecteur de la base de données pour les automates.
    /// </summary>
    internal static class AutomateDbReader
    {
        /// <summary>
        /// Récupère les états liés à un automate à partir des lignes de la base de données.
        /// </summary>
        /// <param name="automate">Automate avec ses propres informations mais sans ses etats</param>
        /// <returns>Un dictionnaire des états de l'automate</returns>
        internal static Dictionary<int, Etat> GetEtatsFromRows(Automate automate)
        {
            Dictionary<int, Etat> result = new Dictionary<int, Etat>();

            using (SQLiteConnector connection = new SQLiteConnector())
            {
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
            }

            return result;
        }

        /// <summary>
        /// Récupère les transitions liées à un automate à partir des lignes de la base de données.
        /// </summary>
        /// <param name="automate">Automate avec ses propres informations mais sans ses transitions</param>
        /// <param name="etatDictionary">Dictionnaire d'états</param>
        internal static void GetTransitionsFromRows(Automate automate, Dictionary<int, Etat> etatDictionary)
        {
            using (SQLiteConnector connection = new SQLiteConnector())
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
}
