using API.Data.Interfaces;
using LogicLayer;

namespace API.Data.Realisations
{
    public class TransitionDAO : ITransitionDAO
    {
        public void InsertTransitions(Automate automate)
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
