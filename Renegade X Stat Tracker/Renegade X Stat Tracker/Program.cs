using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Renegade_X_Stat_Tracker
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        static extern IntPtr RemoveMenu(IntPtr hMenu, uint nPosition, uint wFlags);
        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        const int VK_RETURN = 0x0D;
        const int WM_KEYDOWN = 0x100;

        internal const uint SC_CLOSE = 0xF060;
        internal const uint MF_GRAYED = 0x00000001;
        internal const uint MF_BYCOMMAND = 0x00000000;
        internal const uint MF_ENABLED = 0x00000000;

        public static List<Player> cachedPlayers = new List<Player>();

        public static List<Player> players = new List<Player>();
        public const bool debug = true;
        public static Database db;

        static void Main(string[] args)
        {
            EnableCloseButton(false);

            Util.generatePlayerUpdateQueryString();

            LoadSettings();

            if (!File.Exists(Util.ini.IniReadValue("LOG", "path")))
            {
                Console.WriteLine("Couldn't load log file, check settings.ini and start me again!");
                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
                return;
            }

            db = new Database(
                Util.ini.IniReadValue("DATABASE", "host"), 
                Util.ini.IniReadValue("DATABASE", "user"),
                Util.ini.IniReadValue("DATABASE", "database"),
                Util.ini.IniReadValue("DATABASE", "password"), 
                players
            );

            if(!db.Connected())
            {
                Console.WriteLine("Could not connect to database, check settings.ini and start me again!");
                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Connected to database successfully!");

            LoadCachedPlayers();
            Util.ClearCacheStart(10);

            if (RCON.Connect(Util.ini.IniReadValue("RCON", "host"), Convert.ToInt16(Util.ini.IniReadValue("RCON", "port")), Util.ini.IniReadValue("RCON", "password")))
            {
                Util.StartAnnouncements(Convert.ToInt32(Util.ini.IniReadValue("ANNOUNCEMENTS", "delay")));
            }

            Tail tail = new Tail(Util.ini.IniReadValue("LOG", "path"), 1);
            tail.LineFilter = "Rx:";
            tail.Changed += new EventHandler<Tail.TailEventArgs>(NewLine);
            tail.Run();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                var hWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
                PostMessage(hWnd, WM_KEYDOWN, VK_RETURN, 0);
            };

            Console.ReadLine();

            ExitProgram(tail);
        }

        private static void ExitProgram(Tail tail)
        {
            Console.WriteLine("Exiting program...");
            db.Close();
            tail.Stop();
            RCON.CloseDown();
        }

        private static void LoadCachedPlayers()
        {
            db.LoadOnlinePlayers(cachedPlayers);

            if(debug)
                Console.WriteLine("Loaded " + cachedPlayers.Count + " into cached player list!");
        }

        private static void EnableCloseButton(bool bEnabled)
        {
            IntPtr hMenu = Process.GetCurrentProcess().MainWindowHandle;
            IntPtr hSystemMenu = GetSystemMenu(hMenu, false);

            EnableMenuItem(hSystemMenu, SC_CLOSE, bEnabled ? MF_ENABLED : MF_GRAYED);
            RemoveMenu(hSystemMenu, SC_CLOSE, MF_BYCOMMAND);
        }

        static void LoadSettings()
        {
            if (!File.Exists("settings.ini"))
            {
                Util.ini.IniWriteValue("LOG", "path", "log.txt");

                Util.ini.IniWriteValue("DATABASE", "host", "127.0.0.1");
                Util.ini.IniWriteValue("DATABASE", "database", "renegade");
                Util.ini.IniWriteValue("DATABASE", "user", "root");
                Util.ini.IniWriteValue("DATABASE", "password", "");

                Util.ini.IniWriteValue("RCON", "host", "127.0.0.1");
                Util.ini.IniWriteValue("RCON", "port", "7777");
                Util.ini.IniWriteValue("RCON", "password", "");
            }
        }

        static void NewLine(object sender, Tail.TailEventArgs e)
        {
            Match join = Regex.Match(e.Line, "PLAYER:\\s\\\"(.*?)\\\".*?\\sentered\\sfrom\\s(.*?)\\ssteamid\\s(.*?)\\n", RegexOptions.IgnoreCase);
            Match chat = Regex.Match(e.Line, "CHAT:\\s\"(.*?)\".*?say\\s\"(.*?)\"", RegexOptions.IgnoreCase);
            Match quit = Regex.Match(e.Line, "PLAYER:\\s\"(.*?)\".*?disconnected", RegexOptions.IgnoreCase);
            Match kill = Regex.Match(e.Line, "GAME:\\s\"(.*?)\".*?\\skilled\\s\"(.*?)\".*?\\swith\\s\"(.*?)\"", RegexOptions.IgnoreCase);
            Match building = Regex.Match(e.Line, "GAME:\\s\"(.*?)\".*?\\sdestroyed_building\\s\"(.*?)\"\\swith\\s\"(.*?)\"", RegexOptions.IgnoreCase);
            Match destroyed = Regex.Match(e.Line, "GAME:\\s\"(.*?)\".*?\\sdestroyed\\s\"(.*?)\"\\swith\\s\"(.*?)\"",RegexOptions.IgnoreCase);

            if (kill.Success)
                OnPlayerKillPlayer(kill);
            else if (destroyed.Success)
                OnPlayerDestroyVehicle(destroyed);
            else if (building.Success)
                OnPlayerKillBuilding(building);
            else if (chat.Success)
                OnPlayerChat(chat);
            else if (join.Success)
                OnPlayerJoin(join);
            else if (quit.Success)
                OnPlayerQuit(quit);
        }

        public static void OnPlayerDestroyVehicle(Match destroyed)
        {
            Player killer = FindPlayerByName(destroyed.Groups[1].ToString());

            if (killer != null)
            {
                killer.destroyed++;
                killer.AddExperience(350);
            }
        }

        public static void OnPlayerKillBuilding(Match building)
        {
            Player killer = FindPlayerByName(building.Groups[1].ToString());

            if (killer != null)
            {
                killer.buildings++;
                killer.AddExperience(1000);
            }

            int building_id = Util.FindBuildingID(building.Groups[2].ToString());

            if (building_id == 0)
                Console.WriteLine("Unknown building: " + building.Groups[2].ToString());
            else
            {
                if (killer != null)
                    db.InsertBuildingKill(killer, building_id);
            }
        }

        public static void OnPlayerKillPlayer(Match kill)
        {
            Player killer = FindPlayerByName(kill.Groups[1].ToString());
            Player victim = FindPlayerByName(kill.Groups[2].ToString());

            int damage = Util.FindWeaponID(kill.Groups[3].ToString());

            if (killer != null)
                killer.HandleKill(damage);

            if (victim != null)
                victim.HandleDeath(damage);

            if (damage == 0)
                Console.WriteLine("Unknown weapon: " + kill.Groups[3].ToString());
            else
            {
                if (victim != null || killer != null)
                    db.InsertKill(killer, victim, damage);
            }
        }

        public static void OnPlayerQuit(Match quit)
        {
            Player p = FindPlayerByName(quit.Groups[1].ToString());

            if (p != null)
            {
                players.Remove(p);

                p.online = 0;

                p.Update();

                if (debug)
                    Console.WriteLine("Removed player: " + p.name);
            }
        }

        public static Player FindPlayerByName(string name)
        {
            Player p = players.Find(r => r.name.Equals(name.ToString()));

            if(p == null)
            {
                p = cachedPlayers.Find(r => r.name.Equals(name.ToString()));

                if (p != null)
                {
                    players.Add(p);
                    cachedPlayers.Remove(p);

                    Console.WriteLine("Now tracking previously untracked player: " + name);
                }
            }

            return p;
        }

        public static void OnPlayerChat(Match chat)
        {
            Player p = FindPlayerByName(chat.Groups[1].ToString());

            if (p != null)
            {
                p.spoken++;

                if(RCON.tcp.Connected)
                    Command.Parse(chat.Groups[2].ToString(), p);
            }

            if(debug)
                Console.WriteLine(chat.Groups[1] + ": " + chat.Groups[2]);
        }

        public static void OnPlayerJoin(Match join)
        {
            string id = join.Groups[3].ToString().Substring(2).TrimEnd( '\r', '\n' );

            Player p = new Player(
                join.Groups[1].ToString(), 
                join.Groups[2].ToString(), 
                Int64.Parse(id, System.Globalization.NumberStyles.HexNumber)
            );

            Player currPlayer = FindPlayerByName(p.name);

            if (currPlayer != null)
            {
                if (debug)
                    Console.WriteLine("Attempting to remove player " + p.name + " already existed, we're removing his old instance!");

                players.Remove(currPlayer);
            }

            db.RegisterPlayer(p);

            if(p.banned == 1)
            {
                Console.WriteLine("Kicking " + p.name + " because he is banned!");
                RCON.Command("AdminKick " + p.name);
                return;
            }

            p.online = 1;

            players.Add(p);

            if(debug)
                Console.WriteLine("Added player: " + p.name + " with ID: " + p.steamid);
        }
    }
}