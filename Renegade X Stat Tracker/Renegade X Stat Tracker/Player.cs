using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Renegade_X_Stat_Tracker
{
    class Player
    {
        public string name;
        public string ip;
        public Int64 steamid;

        public uint kills {get; set; }
        public uint deaths { get; set; }
        public uint suicides { get; set; }
        public uint killstreak { get; set; }
        public uint deathstreak { get; set; }
        public uint buildings { get; set; }
        public uint destroyed { get; set; }
        public uint headshots { get; set; }
        public uint runover { get; set; }
        public uint level { get; set; }
        public uint spoken { get; set; }
        public sbyte banned { get; set; }
        public sbyte online { get; set; }
        public uint experience { get; set; }
        public uint plevel { get; set; }

        public uint currentkillstreak;
        public uint currentdeathstreak;
        public uint requiredexperience;

        public Player(string name, string ip, Int64 steamid)
        {
            this.name = name;
            this.ip = ip;
            this.steamid = steamid;
        }

        public void AddExperience(uint amount)
        {
            experience += amount;

            if(experience >= requiredexperience)
            {
                experience = experience - requiredexperience;
                plevel++;
                CalculateRequiredExperience();
            }
        }

        public void CalculateRequiredExperience()
        {
            uint basexp = 1000;

            requiredexperience = basexp + (320 * plevel);
        }

        public void HandleKill(int damage)
        {
            kills++;
            currentkillstreak++;

            if (currentkillstreak > killstreak)
                killstreak = currentkillstreak;

            currentdeathstreak = 0;

            uint increase = Convert.ToUInt32(Math.Round(100 * Convert.ToDecimal(currentkillstreak) / 10m));

            AddExperience(100 + increase);

            if (Util.IsHeadShot(damage))
            {
                headshots++;
                AddExperience(30);
            }
        }

        public void HandleDeath(int damage)
        {
            deaths++;
            currentdeathstreak++;

            if (currentdeathstreak > deathstreak)
                deathstreak = currentdeathstreak;

            currentkillstreak = 0;

            if (Util.IsRunOver(damage))
                runover++;
        }

        public void Update()
        {
            using (MySqlConnection conn = new MySqlConnection(Database.dbConnection))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                Player p = this;

                cmd.CommandText = "UPDATE players " + Util.PlayerUpdateQueryString;

                cmd.Prepare();

                for (int i = 0; i < Util.pi.Length; i++)
                    cmd.Parameters.AddWithValue("@" + Util.pi[i].Name, Util.pi[i].GetValue(p));

                cmd.Parameters.AddWithValue("@steamid", p.steamid);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void Register()
        {
            using (MySqlConnection conn = new MySqlConnection(Database.dbConnection))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                Player p = this;

                if (!Exists())
                    cmd.CommandText = "INSERT INTO players (name, steamid, ip) VALUES (@name, @steamid, @ip)";
                else
                    cmd.CommandText = "UPDATE players SET name = @name, ip = @ip WHERE steamid = @steamid";

                cmd.Prepare();

                cmd.Parameters.AddWithValue("@name", p.name);
                cmd.Parameters.AddWithValue("@steamid", p.steamid);
                cmd.Parameters.AddWithValue("@ip", p.ip);

                Stopwatch st = new Stopwatch();
                st.Start();

                Console.WriteLine("Checking if player: " + name + " exists!");

                cmd.ExecuteNonQuery();

                st.Stop();

                Console.WriteLine("Took " + st.ElapsedMilliseconds + "ms");

                conn.Close();
            }

            CalculateRequiredExperience();
        }

        public bool Exists()
        {
            bool result = false;

            using (MySqlConnection conn = new MySqlConnection(Database.dbConnection))
            {
                conn.Open();

                Player p = this;

                string stm = "SELECT " + Util.PlayerReadQueryString + " FROM Players WHERE steamid = @steamid";

                MySqlCommand cmd = new MySqlCommand(stm, conn);

                cmd.Parameters.AddWithValue("@steamid", p.steamid);

                MySqlDataReader rdr = cmd.ExecuteReader();

                result = rdr.HasRows;

                if (rdr.HasRows)
                {
                    rdr.Read();

                    for (int i = 0; i < Util.pi.Length; i++)
                        Util.pi[i].SetValue(p, rdr.GetValue(i));
                }

                rdr.Close();
                conn.Close();
            }

            return result;
        }
    }
}