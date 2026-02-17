using API.Data.Realisations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPI
{
    public class TestSQLiteConnector
    {
        [Fact]
        public void ExecuteInsert_And_Query_ShouldReturnInsertedRow()
        {
            using var connection = new SQLiteConnector(); // config Source=:memory:

            connection.ExecuteNonQuery("CREATE TABLE Users(Id INTEGER PRIMARY KEY, Name TEXT);");

            long id = connection.ExecuteInsert("INSERT INTO Users(Name) VALUES(@Name);",
                new Dictionary<string, object> { { "@Name", "TestUser" } });

            var dt = connection.ExecuteQuery("SELECT * FROM Users WHERE Id=@Id",
                new Dictionary<string, object> { { "@Id", id } });

            Assert.Single(dt.Rows);
            Assert.Equal("TestUser", dt.Rows[0]["Name"]);
        }
    }
}
