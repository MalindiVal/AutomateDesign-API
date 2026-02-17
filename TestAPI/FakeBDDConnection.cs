using API.Data.Interfaces;
using System.Data;

namespace TestAPI
{
    public class FakeBDDConnection : IBDDConnection
    {
        private long _currentId = 1;

        private readonly DataTable _automatesTable = new DataTable();
        private readonly DataTable _usersTable = new DataTable();

        public FakeBDDConnection()
        {
            _automatesTable.Columns.Add("Id", typeof(int));
            _automatesTable.Columns.Add("Nom", typeof(string));
            _usersTable.Columns.Add("Id", typeof(int));
            _usersTable.Columns.Add("Login", typeof(string));
            _usersTable.Columns.Add("Mdp", typeof(string));
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
                _usersTable.Rows.Add(id, parameters["@Login"], parameters["@Mdp"]);
            }

            return id;
        }

        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            if (query.StartsWith("SELECT Id, Nom FROM Automates WHERE"))
            {
                int id = (int)parameters["@Id"];

                var result = _automatesTable.Clone();

                foreach (DataRow row in _automatesTable.Rows)
                {
                    if ((int)row["Id"] == id)
                        result.ImportRow(row);
                }

                return result;
            }

            if (query.StartsWith("SELECT Id, Nom FROM Automates"))
            {
                return _automatesTable.Copy();
            }

            if (query.StartsWith("SELECT Id, Login, Mdp FROM Utilisateurs WHERE"))
            {
                string login = (string)parameters["@Login"];
                var result = _usersTable.Clone();
                foreach (DataRow row in _usersTable.Rows)
                {
                    if ((string)row["Login"] == login)
                        result.ImportRow(row);
                }
                return result;
            }

            return new DataTable();
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            return 1;
        }
    }
}
