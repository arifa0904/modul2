using modul2.RepositoriData;
using System;
using System.Collections.Generic;
using Npgsql;

namespace modul2.Models
{
    public class MuridContext
    {
        private string __constr;
        public string __ErrorMsg { get; private set; }

        public MuridContext(string constr)
        {
            __constr = constr;
        }

        // Register Murid
        public bool RegisterMurid(Murid murid)
        {
            string query = @"INSERT INTO users.murid (nama, alamat, email, password) 
                             VALUES (@name, @alamat, @email, @password);";
            SqlRepositoriData db = new SqlRepositoriData(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", murid.nama);
                cmd.Parameters.AddWithValue("@alamat", murid.alamat);
                cmd.Parameters.AddWithValue("@email", murid.email);
                cmd.Parameters.AddWithValue("@password", murid.password);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
                return true;
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
                return false;
            }
        }

        // Get Murid by Email
        public Murid GetMuridByEmail(string email)
        {
            string query = "SELECT id_murid, nama, alamat, email, password, role FROM murid WHERE email = @Email;";
            SqlRepositoriData db = new SqlRepositoriData(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@Email", email);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Murid()
                    {
                        id_murid = int.Parse(reader["id_murid"].ToString()),
                        nama = reader["nama"].ToString(),
                        alamat = reader["alamat"].ToString(),
                        email = reader["email"].ToString(),
                        password = reader["password"].ToString(),
                        role = reader["role"].ToString()
                    };
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
            return null;
        }

        // List All Murid
        public List<Murid> ListMurid()
        {
            List<Murid> muridList = new List<Murid>();
            string query = "SELECT * FROM murid;";
            SqlRepositoriData db = new SqlRepositoriData(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    muridList.Add(new Murid()
                    {
                        id_murid = int.Parse(reader["id_murid"].ToString()),
                        nama = reader["nama"].ToString(),
                        alamat = reader["alamat"].ToString(),
                        email = reader["email"].ToString(),
                        password = reader["password"].ToString(),
                        role = reader["role"].ToString()
                    });
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
            return muridList;
        }

        // Update Murid
        public bool UpdateMurid(Murid murid)
        {
            string query = @"UPDATE murid SET name = @nama, alamat = @alamat, email = @email, role = @role WHERE id_murid = @id;";
            SqlRepositoriData db = new SqlRepositoriData(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id", murid.id_murid);
                cmd.Parameters.AddWithValue("@name", murid.nama);
                cmd.Parameters.AddWithValue("@alamat", murid.alamat);
                cmd.Parameters.AddWithValue("@email", murid.email);
                cmd.Parameters.AddWithValue("@role", murid.role);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
                return true;
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
                return false;
            }
        }

        // Delete Murid
        public bool DeleteMurid(int id)
        {
            string query = "DELETE FROM murid WHERE id_murid = @id;";
            SqlRepositoriData db = new SqlRepositoriData(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
                return true;
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
                return false;
            }
        }
    }
}
