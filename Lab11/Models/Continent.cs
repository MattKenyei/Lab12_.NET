using Npgsql;
using System;
using System.Data;
using System.Drawing;

namespace Lab11.Models
{
    public class Continent
    {
        private static readonly DataSet ds = new DataSet();
        
       
        private static readonly string _insertCountryCommand = "INSERT INTO  sport_kinds (name) VALUES ( @Title)";
        private static readonly string _updateCountryCommand = "UPDATE sport_kinds SET name=@_title WHERE id = @_id;";
        private static readonly string _deleteCountryCommand = "DELETE FROM sport_kinds WHERE id = @_id;";
        public int Id { get; set; }
       
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

        public static void Insert( Continent continent)
        {
            DataTable dt = new DataTable();
            NpgsqlConnection con1 = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********");
            con1.Open();
            string sql = ("SELECT * FROM sport_kinds");
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con1);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            con1.Close();
            DataRow newRow = dt.NewRow();
            newRow["name"] = continent.Name;
            using (NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********"))
            {
                con.Open();
                NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(_insertCountryCommand, con);
                da1.InsertCommand = new NpgsqlCommand(_insertCountryCommand, con);
                da1.InsertCommand.Parameters.AddWithValue("@Title", continent.Name);
                dt.Rows.Add(newRow);
                da1.Update(dt);


                con.Close();
            }
        }
        public static void Update(Continent continent)
        {
            DataTable dt = new DataTable();
            NpgsqlConnection con1 = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********");
            con1.Open();
            string sql = ("SELECT * FROM sport_kinds");
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con1);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            con1.Close();
            DataRow newRow = dt.NewRow();
            newRow["name"] = continent.Name;
            using (NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********"))
            {
                con.Open();
                NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(_updateCountryCommand, con);
                da1.InsertCommand = new NpgsqlCommand(_updateCountryCommand, con);
                da1.InsertCommand.Parameters.AddWithValue("@_title", continent.Name);
                da1.InsertCommand.Parameters.AddWithValue("@_id", continent.Id);
                dt.Rows.Add(newRow);
                da1.Update(dt);


                con.Close();
            }
        }
        public static void Delete(Continent continent)
        {
            DataTable dt = new DataTable();
            NpgsqlConnection con1 = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********");
            con1.Open();
            string sql = ("SELECT * FROM sport_kinds");
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con1);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            con1.Close();
            DataRow newRow = dt.NewRow();
            newRow["name"] = continent.Name;
            using (NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********"))
            {
                con.Open();
                NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(_deleteCountryCommand, con);
                da1.InsertCommand = new NpgsqlCommand(_deleteCountryCommand, con);
            
                da1.InsertCommand.Parameters.AddWithValue("@_id", continent.Id);
                dt.Rows.Add(newRow);
                da1.Update(dt);


                con.Close();
            }
        }




    }
}
