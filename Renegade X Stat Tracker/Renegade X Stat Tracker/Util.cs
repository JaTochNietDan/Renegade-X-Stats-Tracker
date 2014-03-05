using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Renegade_X_Stat_Tracker
{
    class Util
    {
        private static Timer announcement, reannouncement, clearCache;

        private static string lastAnnouncement;

        public static IniFile ini = new IniFile("settings.ini");

        public static PropertyInfo[] pi = typeof(Player).GetProperties();

        private static Random rand = new Random();

        public static readonly Dictionary<string, int> damageSources = new Dictionary<string, int>
        {
            {"DamageType", -1},
            {"Rx_DmgType_AutoRifle", 1},
            {"Rx_DmgType_MammothTank_Missile", 2},
            {"Rx_DmgType_RamjetRifle", 3},
            {"Rx_DmgType_FireBleed", 4},
            {"Rx_DmgType_Headshot", 5},
            {"Rx_Vehicle_Humvee_DmgType", 6},
            {"Rx_DmgType_MRLS", 7},
            {"Rx_DmgType_LightTank", 8},
            {"Rx_DmgType_Artillery", 9},
            {"Rx_DmgType_RanOver", 10},
            {"Rx_DmgType_Pancake", 11},
            {"Rx_DmgType_TiberiumBleed", 12},
            {"Rx_DmgType_TacticalRifle", 13},
            {"Rx_DmgType_MissileLauncher", 14},
            {"Rx_Vehicle_Buggy_DmgType", 15},
            {"Rx_Vehicle_APC_GDI_DmgType", 16},
            {"Rx_DmgType_MediumTank", 17},
            {"Rx_DmgType_SniperRifle", 18},
            {"Rx_DmgType_StealthTank", 19},
            {"DmgType_Suicided", 20},
            {"UTDmgType_VehicleExplosion", 21},
            {"Rx_DmgType_MarksmanRifle", 22},
            {"Rx_DmgType_LaserChainGun", 23},
            {"Rx_DmgType_LaserRifle", 24},
            {"Rx_DmgType_GrenadeLauncher", 25},
            {"Rx_DmgType_FlameTank", 26},
            {"Rx_DmgType_FlakCannon", 27},
            {"Rx_DmgType_MammothTank_Cannon", 28},
            {"Rx_DmgType_Railgun", 29},
            {"Rx_DmgType_PersonalIonCannon", 30},
            {"Rx_Vehicle_APC_Nod_DmgType", 31},
            {"Rx_DmgType_TimedC4", 32},
            {"Rx_DmgType_Apache_Rocket", 33},
            {"Rx_DmgType_FlakCannon_Alt", 34},
            {"Rx_DmgType_RocketLauncher", 35},
            {"Rx_DmgType_ChemicalThrower", 36},
            {"Rx_DmgType_Orca_Missile", 37},
            {"Rx_DmgType_Apache_Gun", 38},
            {"DmgType_Fell", 39},
            {"Rx_DmgType_Shotgun", 40},
            {"KillZDamageType", 41}
        };

        public static string PlayerReadQueryString, PlayerUpdateQueryString;

        public static readonly Dictionary<string, int> buildings = new Dictionary<string, int>
        {
            {"Hand of Nod", 1},
            {"Airstrip", 2},
            {"Weapons Factory", 3},
            {"Power Plant", 4},
            {"Infantry Barracks", 5},
            {"Refinery", 6},
            {"Advanced Guard Tower", 7},
            {"Obelisk", 8}
        };

        public static bool IsHeadShot(int id)
        {
            return id == 5;
        }

        public static bool IsRunOver(int id)
        {
            return id == 10;
        }

        public static int FindWeaponID(string name)
        {
            return damageSources.ContainsKey(name) ? damageSources[name] : 0;
        }

        public static int FindBuildingID(string name)
        {
            return buildings.ContainsKey(name) ? buildings[name] : 0;
        }

        public static void Announce(string message)
        {
            RCON.say(message);
        }

        public static void ClearCacheStart(int delay)
        {
            clearCache = new Timer((delay * 1000) * 60);

            clearCache.Elapsed += ClearCache;
            clearCache.Start();
        }

        private static void ClearCache(object sender, ElapsedEventArgs e)
        {
            foreach (Player p in Program.cachedPlayers)
                p.online = 0;

            Program.db.UpdateListOfPlayers(Program.cachedPlayers);

            Program.cachedPlayers.Clear();

            Console.WriteLine("Cleared the cached players, they are assumed offline!");
            clearCache.Stop();
        }

        public static void StartAnnouncements(int delay)
        {
            announcement = new Timer(delay * 1000);
            reannouncement = new Timer(2000);

            announcement.Elapsed += MakeAnnouncement;
            announcement.Start();

            reannouncement.Elapsed += Reannouncement;
            reannouncement.Start();

            MakeAnnouncement(null, null);
        }

        public static void StopAnnouncements()
        {
            announcement.Stop();
            reannouncement.Stop();
        }

        private static void Reannouncement(object sender, ElapsedEventArgs e)
        {
            Announce(lastAnnouncement);
        }

        private static void MakeAnnouncement(object sender, ElapsedEventArgs e)
        {
            string v = rand.Next(1, Convert.ToInt16(ini.IniReadValue("ANNOUNCEMENTS", "total")) + 1).ToString();

            string announcement = ini.IniReadValue("ANNOUNCEMENTS", v);

            if (announcement != null)
            {
                lastAnnouncement = "[ANNOUNCEMENT]: " + ini.IniReadValue("ANNOUNCEMENTS", v);

                Announce(lastAnnouncement);
            }
        }

        public static void generatePlayerUpdateQueryString()
        {
            for (int i = 0; i < pi.Length; i++)
            {
                PlayerReadQueryString += pi[i].Name;

                if (i != pi.Length - 1)
                    PlayerReadQueryString += ", ";

                if (i == 0)
                    PlayerUpdateQueryString += "SET ";

                PlayerUpdateQueryString += pi[i].Name + " = @" + pi[i].Name;

                if (i < pi.Length - 1)
                    PlayerUpdateQueryString += ", ";
            }

            PlayerUpdateQueryString += " WHERE steamid = @steamid";
        }
    }
}
