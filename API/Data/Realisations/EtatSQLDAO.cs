using API.Data.Interfaces;
using LogicLayer;
using System.Data;

namespace API.Data.Realisations
{
    public class EtatSQLDAO : IEtatDAO
    {
        private readonly IBDDConnection connection;

        public EtatSQLDAO(IBDDConnection connection)
        {
            this.connection = connection;
        }

        public void InsertEtats(Automate automate)
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

        public List<Etat> GetEtatsByAutomate(int id)
        {
            
            List<Etat> results = new List<Etat>();

            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
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
                results.Add(e);
            }

            return results;
        }
    }
}
