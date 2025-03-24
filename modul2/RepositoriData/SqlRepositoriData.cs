using modul2.Models;
using Npgsql;
using System.Data;

namespace modul2.RepositoriData
{
    public class SqlRepositoriData
    {
        private NpgsqlConnection connection;
        private string __constr;
        public SqlRepositoriData(string pCOnstr)
        {
            __constr = pCOnstr;
            connection = new NpgsqlConnection();
            connection.ConnectionString = __constr;
        }

        public NpgsqlCommand getNpgsqlCommand(string query)
        {
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        public void closeConnection()
        {
            connection.Close();
        }
    }
}
