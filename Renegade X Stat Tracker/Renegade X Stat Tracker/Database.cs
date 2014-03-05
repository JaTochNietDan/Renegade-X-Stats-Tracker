using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Renegade_X_Stat_Tracker
{
    class Database
    {
        private Timer updatePlayers;

        private List<Player> players;

        public static string dbConnection {get; private set;}

        private bool connected = false;

        public Database(string ip, string user, string db, string pass, List<Player> players)
        {
            this.players = players;

            dbConnection = "server=" + ip + ";uid=" + user + ";pwd=" + pass + ";database=" + db;

            using (MySqlConnection conn = new MySqlConnection(dbConnection))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                updatePlayers = new Timer(10000);

                updatePlayers.Elapsed += updatePlayers_Elapsed;

                updatePlayers.Start();

                connected = true;
            }
        }

        // Probably deprecated
        /*private void UpdateDamageTable()
        {
            MySqlTransaction trTmp = conn.BeginTransaction();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Transaction = trTmp;

            foreach(KeyValuePair<string, int> i in Util.damageSources)
            {
                cmd.CommandText = "REPLACE INTO `damage` (id, value) VALUES (@id, @value)";

                cmd.Prepare();

                cmd.Parameters.AddWithValue("@id", i.Value);
                cmd.Parameters.AddWithValue("@value", i.Key);

                cmd.ExecuteNonQuery();

                cmd.Parameters.RemoveAt("@id");
                cmd.Parameters.RemoveAt("@value");
            }

            trTmp.Commit();
        }*/

        public void InsertKill(Player killer, Player victim, int damage)
        {
            using (MySqlConnection conn = new MySqlConnection(dbConnection))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                Int64 killer_id = killer == null ? 0 : killer.steamid;
                Int64 victim_id = victim == null ? 0 : victim.steamid;

                cmd.CommandText = "INSERT INTO `kills` (killer, victim, damage) VALUES (@killer, @victim, @damage)";

                cmd.Prepare();

                cmd.Parameters.AddWithValue("@killer", killer_id);
                cmd.Parameters.AddWithValue("@victim", victim_id);
                cmd.Parameters.AddWithValue("@damage", damage);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void UnbanPlayerById(Int64 id)
        {
            using (MySqlConnection conn = new MySqlConnection(dbConnection))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = "UPDATE `players` SET banned = 0 WHERE steamid = @steamid LIMIT 1";

                cmd.Prepare();

                cmd.Parameters.AddWithValue("@steamid", id);

                cmd.ExecuteNonQuery();
            }
        }

        public void InsertBuildingKill(Player killer, int building)
        {
            using (MySqlConnection conn = new MySqlConnection(dbConnection))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = "INSERT INTO `building_kills` (killer, building) VALUES (@killer, @building)";

                cmd.Prepare();

                cmd.Parameters.AddWithValue("@killer", killer.steamid);
                cmd.Parameters.AddWithValue("@building", building);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public bool Connected()
        {
            return connected;
        }

        public void RegisterPlayer(Player p)
        {
            p.Register();
        }

        public void Close()
        {
            updatePlayers.Stop();

            updatePlayers_Elapsed(this, null);
        }

        public void LoadOnlinePlayers(List<Player> players)
        {
            using (MySqlConnection conn = new MySqlConnection(dbConnection))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = "SELECT * FROM `players` WHERE online = 1";

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Player p = new Player(rdr.GetString("name"), rdr.GetString("ip"), rdr.GetInt64("steamid"));

                    for (int i = 0; i < Util.pi.Length; i++)
                        Util.pi[i].SetValue(p, rdr.GetValue(i + 3));

                    p.CalculateRequiredExperience();

                    players.Add(p);
                }

                rdr.Close();
                conn.Close();
            }
        }

        private void updatePlayers_Elapsed(object source, ElapsedEventArgs e)
        {
            UpdateListOfPlayers(players);
        }

        public void UpdateListOfPlayers(List<Player> players)
        {
            using(MySqlConnection conn = new MySqlConnection(dbConnection))
            {
                conn.Open();

                MySqlTransaction tr = conn.BeginTransaction();

                foreach(Player p in players)
                {
                    MySqlCommand cmd = new MySqlCommand();

                    cmd.Connection = conn;
                    cmd.Transaction = tr;

                    cmd.CommandText = "UPDATE players " + Util.PlayerUpdateQueryString;

                    cmd.Prepare();

                    for (int i = 0; i < Util.pi.Length; i++)
                        cmd.Parameters.AddWithValue("@" + Util.pi[i].Name, Util.pi[i].GetValue(p));

                    cmd.Parameters.AddWithValue("@steamid", p.steamid);

                    cmd.ExecuteNonQuery();
                }

                tr.Commit();
                conn.Close();
            }
        }
    }
}
