using API.Data.Interfaces;
using System.Data;

namespace TestAPI
{
    public class FakeBDDConnection : IBDDConnection
    {
        private long _currentId = 1;

        private readonly DataTable _automatesTable = new DataTable();

        public FakeBDDConnection()
        {
            _automatesTable.Columns.Add("Id", typeof(int));
            _automatesTable.Columns.Add("Nom", typeof(string));
        }

        public long ExecuteInsert(string query, Dictionary<string, object> parameters = null)
        {
            int id = (int)_currentId++;

            if (query.Contains("INSERT INTO Automates"))
            {
                _automatesTable.Rows.Add(id, parameters["@Nom"]);
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

            return new DataTable();
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            return 1;
        }
    }
}
