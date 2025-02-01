using FootBall.Connexion;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootBall.Entity
{
    internal class Equipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Couleur { get; set; }
        public string Direction {  get; set; }
        public int score { get; set; }

        public int arret {  get; set; }

        public Equipe() { }

        public Equipe(int id, string name, string couleur)
        {
            this.Id = id;
            this.Name = name;
            this.Couleur = couleur;
        }

        public static List<Equipe> GetEquipes()
        {
            List<Equipe> equipes = new List<Equipe>();

            string query = "SELECT * FROM equipe";

            using (NpgsqlConnection conn = new NpgsqlConnection(SeConnecter.dataSource()))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Equipe equipe = new Equipe
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = Convert.ToString(reader["name"]),
                                Couleur = Convert.ToString(reader["couleur"]),
                                score = reader["score"] != DBNull.Value ? Convert.ToInt32(reader["score"]) : 0
                            };

                            equipes.Add(equipe);
                        }
                    }
                }
            }

            return equipes;
        }


        public static Equipe GetEquipe(string couleur)
        {
            Equipe equipe = null;

            try
            {
                using (var conn = new NpgsqlConnection(SeConnecter.dataSource()))
                {
                    conn.Open();

                    string query = "SELECT * FROM equipe WHERE couleur = @couleur";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@couleur", couleur);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                equipe = new Equipe
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    Couleur = reader.GetString(reader.GetOrdinal("couleur")),
                                    score = reader.GetInt32(reader.GetOrdinal("score"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return equipe;
        }

        public void UpdateEquipe()
        {
            try
            {
                using (var conn = new NpgsqlConnection(SeConnecter.dataSource()))
                {
                    conn.Open();

                    string query = "UPDATE Equipe SET score = @Score Where id = @Id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Score", this.score);
                        cmd.Parameters.AddWithValue("@Id", this.Id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void UpdateArretGoal()
        {
            try
            {
                using (var conn = new NpgsqlConnection(SeConnecter.dataSource()))
                {
                    conn.Open();

                    string query = "UPDATE Equipe SET arret = @Arret Where id = @Id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Arret", this.score);
                        cmd.Parameters.AddWithValue("@Id", this.Id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
