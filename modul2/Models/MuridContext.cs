using modul2.RepositoriData;
using System;
using System.Collections.Generic;
using Npgsql;

namespace modul2.Models
{
    public class MuridContext
    {
        private string __constr;
        public string __ErrorMsg;

        public MuridContext(string pObs)
        {
            __constr = pObs;
        }

        // create
        public void AddMurid(Murid murid)
        {
            string query = @"INSERT INTO murid (nama,alamat, email) VALUES (@name, @alamat, @email);";
            SqlRepositoriData db = new SqlRepositoriData(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", murid.nama);
                cmd.Parameters.AddWithValue("@alamat", murid.alamat);
                cmd.Parameters.AddWithValue("@email", murid.email);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;    
            }
        }

        // Get Murid by Email
        public Murid GetMuridByEmail(string email)

        {
            Murid murid = null;
            string query = string.Format(@"SELECT * FROM murid m 
                                         JOIN peran_murid pm ON pm.id_murid = m.id_murid 
                                         JOIN peran p ON p.id_peran = pm.id_peran 
                                         WHERE m.email = @email ");
            SqlRepositoriData db = new SqlRepositoriData(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@email", email);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    murid = new Murid()
                    {
                        id_murid = int.Parse(reader["id_murid"].ToString()),
                        nama = reader["nama"].ToString(),
                        alamat = reader["alamat"].ToString(),
                        email = reader["email"].ToString(),
                        password = reader["password"].ToString(),
                        nama_peran = reader["nama_peran"].ToString()
                    };
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
            return murid;
        }

        // List All Murid
        public List<Murid> ListMurid()
        {
            List<Murid> muridList = new List<Murid>();
            string query = @"SELECT id_murid, nama, alamat, email, password  FROM murid;";
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
        public void UpdateMurid(Murid murid)
        {
            string query = @"UPDATE murid SET nama = @nama, alamat = @alamat, email = @email WHERE id_murid = @id;";
            SqlRepositoriData db = new SqlRepositoriData(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id", murid.id_murid);
                cmd.Parameters.AddWithValue("@nama", murid.nama);
                cmd.Parameters.AddWithValue("@alamat", murid.alamat);
                cmd.Parameters.AddWithValue("@email", murid.email);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
               
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
                
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
