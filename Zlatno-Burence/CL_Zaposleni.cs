﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Zlatno_Burence
{
    internal class CL_Zaposleni
    {
        //promenjive
        private int id;
        private string ime;
        private string prezime;

        //geteri i seteri
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Ime
        {
            get { return ime; }
            set
            {
                if (value == "")  throw new Exception("Morate uneti ime zaposlenog!"); 
                ime = value;
            }
        }
        public string Prezime 
        {
            get { return prezime; }
            set 
            {
               if (value == "")  throw new Exception("Morate uneti prezime zaposlenog!"); 
                prezime = value; 
            }
        }

        //konekcioni string
        
        private string _connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\L\\Desktop\\faks\\si1\\seminarski\\Zlatno-Burence\\Zlatno-Burence\\ZlatnoBurence.mdf;Integrated Security=True";

        //funkcije za manipulisanje bazom
        public void dodajZaposlenog()
        {
            try
            {
                string insertSql = "INSERT INTO Zaposleni " + "(ImeZaposlenog, PrezimeZaposlenog) VALUES" + "(@Ime, @Prezime)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.Add(new SqlParameter("@Ime", Ime));
                    command.Parameters.Add(new SqlParameter("@Prezime", Prezime));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void azurirajZaposlenog() 
        {
            string updateSql = "UPDATE Zaposleni " + "SET ImeZaposlenog= @Ime, PrezimeZaposlenog=@Prezime " + "WHERE Id = @Id;";
            using (SqlConnection connection = new SqlConnection(_connectionString)) 
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = updateSql;
                command.Parameters.Add(new SqlParameter("@Id", ID));
                command.Parameters.Add(new SqlParameter("@Ime", Ime));
                command.Parameters.Add(new SqlParameter("@Prezime", Prezime));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void obrisiZaposlenog() 
        {
            try
            {
                string deleteSql = "DELETE FROM Zaposleni WHERE Id = @Id;";
                using (SqlConnection connection = new SqlConnection(_connectionString)) 
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deleteSql;
                    command.Parameters.Add(new SqlParameter("@Id", ID));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<CL_Zaposleni> ucitajZaposlene() 
        {
            List<CL_Zaposleni> zaposleni = new List<CL_Zaposleni>();
            string queryString = "SELECT * FROM Zaposleni;";
            using (SqlConnection connection = new SqlConnection(_connectionString)) 
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader()) 
                {
                    CL_Zaposleni zap;
                    while (reader.Read()) 
                    {
                        zap = new CL_Zaposleni();
                        zap.ID = Int32.Parse(reader["Id"].ToString());
                        zap.Ime = reader["ImeZaposlenog"].ToString();
                        zap.Prezime = reader["PrezimeZaposlenog"].ToString();
                        zaposleni.Add(zap);
                    }
                }
            }
            return zaposleni;
        }

    }
}
