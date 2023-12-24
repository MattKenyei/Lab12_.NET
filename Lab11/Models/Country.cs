using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;

namespace Lab11.Models
{
    
    public class Country
    {
        private static readonly DataSet ds = new DataSet();
        private static readonly string _selectCountryCommand = "SELECT * FROM sport_clubs";
        private static readonly string _insertCountryCommand = "INSERT INTO sport_clubs (name, photo , kind_name) VALUES (@name, @Photo , @kindId)";

        private static readonly string _updateCountryCommand = "UPDATE sport_clubs SET name = @_name,Photo=@_photo WHERE id = @_id;";
        private static readonly string _deleteCountryCommand = "DELETE FROM sport_clubs WHERE id = @_id;";
        private static NpgsqlConnection _connection = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********");
        public int Id { get; set; }
        public string Name { get; set; }
        public string Capital { get; set; }
        public string ContinentId { get; set; }
        public byte[] Photo { get; set; }


        public static void Insert(NpgsqlConnection connection, Country user)
        {
            DataTable dt = new DataTable();
            NpgsqlConnection con1 = _connection;
            con1.Open();
            string sql = ("SELECT * FROM sport_clubs");
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con1);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            con1.Close();
            DataRow newRow = dt.NewRow();
            newRow["name"] = user.Name;
            using (NpgsqlConnection con = _connection)
            {
                con.Open();
                NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(_insertCountryCommand, con);
                da1.InsertCommand = new NpgsqlCommand(_insertCountryCommand, con);
                da1.InsertCommand.Parameters.AddWithValue("@name", user.Name);
                da1.InsertCommand.Parameters.AddWithValue("@KindId", user.ContinentId);
                da1.InsertCommand.Parameters.AddWithValue("@Photo", user.Photo);
                dt.Rows.Add(newRow);
                da1.Update(dt);


                con.Close();
            }
        }

        public static void Update(NpgsqlConnection connection, Country user)
        {
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                try
                {
                    connection.Open();
                    var cmd = new NpgsqlCommand(_updateCountryCommand, connection);
                    cmd.Parameters.AddWithValue("@_id",user.Id );
                    cmd.Parameters.AddWithValue("@_name", user.Name);
                    cmd.Parameters.AddWithValue("@KindId", user.ContinentId);
                    NpgsqlParameter photoParam = new NpgsqlParameter
                    {
                        ParameterName = "_photo",
                        NpgsqlDbType = NpgsqlDbType.Bytea,
                        Value = user.Photo
                    };
                    cmd.Parameters.Add(photoParam);
                    var reader = cmd.ExecuteNonQuery();
                    connection.Close();
                }
                finally
                {
                    if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                }
            }
        }

        public static void Delete(NpgsqlConnection connection, int userId)
        {
            DataTable dt = new DataTable();
            NpgsqlConnection con1 = _connection;
            connection.Close();
            con1.Open();
            string sql = ("SELECT * FROM sport_clubs");
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con1);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            con1.Close();
            DataRow newRow = dt.NewRow();
            newRow["name"] = "тест";
            using (NpgsqlConnection con = _connection)
            {
                con.Open();
                NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(_deleteCountryCommand, con);
                da1.InsertCommand = new NpgsqlCommand(_deleteCountryCommand, con);

                da1.InsertCommand.Parameters.AddWithValue("@_id", userId);
                dt.Rows.Add(newRow);
                da1.Update(dt);


                con.Close();
            }
        }
    }
}
