using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renegade_X_Stat_Tracker
{
    class Command
    {
        private static Dictionary<string, Action<string[]>> cmds = new Dictionary<string, Action<string[]>>
        {
            {"kick", Kick},
            {"ban", Ban},
            {"unban", Unban},
            {"setlevel", SetLevel},
            {"addbots", AddBots},
            {"killbots", KillBots},
            {"restartmap", RestartMap},
            {"changemap", ChangeMap},
            {"mute", Mute},
            {"unmute", UnMute}
        };

        private static Dictionary<string, int> cmdsLevel = new Dictionary<string, int>
        {
            {"kick", 2},
            {"ban", 3},
            {"unban", 3},
            {"setlevel", 4},
            {"addbots", 3},
            {"killbots", 3},
            {"restartmap", 3},
            {"changemap", 3},
            {"mute", 1},
            {"unmute", 1}
        };

        public static void Parse(String text, Player p)
        {
            string cmd = GetCommand(text).Substring(1);

            if (!IsCommand(text) || !cmds.ContainsKey(cmd) || p.level < cmdsLevel[cmd])
            {
                return;
            }

            string[] parameters = null;

            if (text.IndexOf(' ') != -1)
                parameters = text.Substring(cmd.Length + 2).Split(' ');

            File.AppendAllText("admin.log", "[ADMIN]: " + p.name + " (" + p.level + ")" + "ran command: " + text + "\n");

            cmds[cmd].Invoke(parameters);
        }

        private static void Mute(string[] args)
        {
            if (args[0] != null)
                RCON.Command("AdminForceTextMute " + args[0]);
        }

        private static void UnMute(string[] args)
        {
            if (args[0] != null)
                RCON.Command("AdminForceTextUnMute " + args[0]);
        }

        private static void RestartMap(string[] args)
        {
            RCON.Command("AdminRestartMap");
        }

        private static void ChangeMap(string[] args)
        {
            if(args[0] != null)
                RCON.Command("AdminChangeMap " + args[0]);
        }

        private static void Kick(string[] args)
        {
            if (args[0] != null)
                RCON.Command("AdminKick " + args[0]);
        }

        private static void Ban(string[] args)
        {
            Player p = null;

            try
            {
                p = Program.FindPlayerByName(args[0]);
            }
            catch (Exception)
            {
                return;
            }

            if (p != null)
            {
                p.banned = 1;
                RCON.Command("AdminKick " + args[0]);
            }
        }

        private static void AddBots(string[] args)
        {
            int bots = 0;

            try
            {
                bots = Convert.ToInt32(args[0]);
            }
            catch (Exception)
            {
                return;
            }

            if (bots != 0)
                RCON.Command("AddBots " + bots);
        }

        private static void KillBots(string[] args)
        {
            RCON.Command("KillBots");
        }

        private static void Unban(string[] args)
        {
            Int64 id = 0;

            try
            {
                id = Convert.ToInt64(args[0]);
            }
            catch(Exception)
            {
                return;
            }

            if(id != 0)
                Program.db.UnbanPlayerById(id);
        }

        private static void SetLevel(string[] args)
        {
            int level = 0;
            Player p = null;

            try
            {
                level = Convert.ToInt32(args[1]);
                p = Program.FindPlayerByName(args[0]);
            }
            catch (Exception)
            {
                return;
            }

            if (level >= 0 && level < 5 && p != null)
                p.level = Convert.ToUInt32(level);
        }
        
        public static bool IsCommand(String text)
        {
            return text.Length > 0 && text[0] == '!';
        }

        public static string GetCommand(string txt)
        {
            int index = txt.IndexOf(' ');
            string command = null;

            if (index == -1) command = txt.Substring(0, txt.Length);
            else command = txt.Substring(0, index);

            return command;
        }
    }
}
