using API.Data.Interfaces;
using LogicLayer;

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
    }
}
