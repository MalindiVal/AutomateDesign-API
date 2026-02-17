using API.Data.Interfaces;
using LogicLayer;
using System.Data;

namespace API.Data.Realisations
{
    public class TransitionSQLDAO : ITransitionDAO
    {
        private readonly IBDDConnection connection;

        public TransitionSQLDAO(IBDDConnection connection)
        {
            this.connection = connection;
        }

        public void InsertTransitions(Automate automate)
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

        public List<Transition> GetTransitionsByAutomate(int id, Dictionary<int, Etat> etatDictionary)
        {
            List<Transition> transitionsList = new List<Transition>();
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
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

                    transitionsList.Add(t);

                }
            }

            return transitionsList;
        }
    }
}
