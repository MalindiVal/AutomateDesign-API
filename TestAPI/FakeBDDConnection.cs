using API.Data.Interfaces;
using System.Data;

namespace TestAPI
{
    public class FakeBDDConnection : IBDDConnection
    {
        private long _currentId = 1;

        private readonly DataTable _automatesTable = new DataTable();
        private readonly DataTable _usersTable = new DataTable();
        private readonly DataTable _etatsTable = new DataTable();
        private readonly DataTable _passwordsTable = new DataTable();
        private readonly DataTable _transitionsTable = new DataTable();

        public FakeBDDConnection()
        {
            _automatesTable.Columns.Add("Id", typeof(int));
            _automatesTable.Columns.Add("Nom", typeof(string));
            _usersTable.Columns.Add("Id", typeof(int));
            _usersTable.Columns.Add("Login", typeof(string));;
            _etatsTable.Columns.Add("Id", typeof(int));
            _etatsTable.Columns.Add("Nom", typeof(string));
            _etatsTable.Columns.Add("IdAutomate", typeof(int));
                _etatsTable.Columns.Add("X", typeof(int));
                _etatsTable.Columns.Add("Y", typeof(int));
                _etatsTable.Columns.Add("estInitial", typeof(bool));
                _etatsTable.Columns.Add("estFinal", typeof(bool));
                _transitionsTable.Columns.Add("Condition", typeof(string));
                _transitionsTable.Columns.Add("EtatDebut", typeof(int));
                _transitionsTable.Columns.Add("EtatFinal", typeof(int));
                _transitionsTable.Columns.Add("IdAutomate", typeof(int));
                _transitionsTable.Columns.Add("X", typeof(int));
                _transitionsTable.Columns.Add("Y", typeof(int));
            _passwordsTable.Columns.Add("IdUser", typeof(int));
                _passwordsTable.Columns.Add("Hash", typeof(string));
        }

        public long ExecuteInsert(string query, Dictionary<string, object> parameters = null)
        {
            int id = (int)_currentId++;

            if (query.Contains("INSERT INTO Automates"))
            {
                _automatesTable.Rows.Add(id, parameters["@Nom"]);
            }

            if (query.Contains("INSERT INTO Utilisateurs"))
            {
                _usersTable.Rows.Add(id, parameters["@Login"]);
            }

            if (query.Contains("INSERT INTO Passwords"))
            {
                _passwordsTable.Rows.Add(parameters["@Id"], parameters["@Hash"]);
            }

            if (query.Contains("INSERT INTO Etats"))
            {
                _etatsTable.Rows.Add(id, parameters["@NomEtat"], parameters["@Id"], parameters["@X"], parameters["@Y"], parameters["@Initial"], parameters["@Final"]);
            }

            if (query.Contains("INSERT INTO Transitions"))
            {
                _transitionsTable.Rows.Add(parameters["@NomTransition"], parameters["@Debut"], parameters["@Final"], parameters["@Id"], parameters["@X"], parameters["@Y"]);
            }




            return id;
        }

        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            var result = new DataTable();
            if (query.StartsWith("SELECT Id, Nom FROM Automates WHERE"))
            {
                int id = (int)parameters["@Id"];

                result = _automatesTable.Clone();

                foreach (DataRow row in _automatesTable.Rows)
                {
                    if ((int)row["Id"] == id)
                        result.ImportRow(row);
                }

            }

            if (query.StartsWith("SELECT Id, Nom FROM Automates"))
            {
                result = _automatesTable.Copy();
            }

            if (query.StartsWith("Select u.Id , u.Login, p.Hash FROM Utilisateurs u JOIN Passwords p ON u.id = p.IdUser WHERE"))
            {
                string login = (string)parameters["@Login"];

                // Table résultat correspondant au SELECT
                result = new DataTable();
                result.Columns.Add("Id", typeof(int));
                result.Columns.Add("Login", typeof(string));
                result.Columns.Add("Hash", typeof(string));

                foreach (DataRow userRow in _usersTable.Rows)
                {
                    if ((string)userRow["Login"] == login)
                    {
                        int userId = (int)userRow["Id"];

                        // Cherche le mot de passe correspondant
                        foreach (DataRow passRow in _passwordsTable.Rows)
                        {
                            if ((int)passRow["IdUser"] == userId)
                            {
                                result.Rows.Add(
                                    userId,
                                    userRow["Login"],
                                    passRow["Hash"]
                                );
                            }
                        }
                    }
                }
            }

            if(query.StartsWith("SELECT Nom, Id, X, Y, estInitial, estFinal FROM Etats WHERE"))
            {
                int id = (int)parameters["@Id"];

                result = _etatsTable.Clone();

                foreach (DataRow row in _etatsTable.Rows)
                {
                    if ((int)row["IdAutomate"] == id)
                        result.ImportRow(row);
                }
            }

            if (query.StartsWith("SELECT Condition, EtatDebut, EtatFinal, IdAutomate , X , Y FROM Transitions WHERE"))
            {
                int id = (int)parameters["@Id"];

                result = _transitionsTable.Clone();

                foreach (DataRow row in _transitionsTable.Rows)
                {
                    if ((int)row["IdAutomate"] == id)
                        result.ImportRow(row);
                }

            }

            return result;
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            return 1;
        }
    }
}
