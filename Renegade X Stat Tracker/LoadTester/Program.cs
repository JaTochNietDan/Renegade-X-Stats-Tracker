using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTester
{
    class Program
    {
        static List<string> currPlayers = new List<string>();

        static Dictionary<string, string> players = new Dictionary<string, string>
        {
            {"JaTochNietDan", "0x0110000101B3D268"},
            {"RedCat", "0x011000010204EA26"},
            {"DirtyDoes", "0x0110000100a91a2f"},
            {"Shadalax", "0x0110000100c344d0"},
            {"Malcom X", "0x01100001014d5c94"},
            {"Baker", "0x01100001017f6774"},
            {"Science", "0x0110000101e4e45c"},
            {"MuchFrog", "0x01100001022b97f7"},
            {"Concrete", "0x01100001022ca129"},
            {"Havok", "0x01100001033da0b4"},
            {"Renegade", "0x011000010354e9cb"},
            {"Chair", "0x0110000103c409b3"},
            {"Barely", "0x110000105b77429"},
            {"Xanexe", "0x0110000102223a07"},
            {"Renegadde", "0x011000010334e9cb"},
            {"Chaiwr", "0x0110000103c402b3"},
            {"Barwely", "0x110000101b77429"},
            {"Xanaex", "0x0110000108223b07"}
        };

        static void Main(string[] args)
        {
            Random rand = new Random();

            foreach(KeyValuePair<string, string> kvp in players)
                AddPlayer(kvp.Key, kvp.Value);

            while(true)
            {
                if (rand.Next(0, 100) >= 50)
                {
                    List<string> keyList = new List<string>(players.Keys);
                    string key = keyList[rand.Next(keyList.Count)];

                    AddPlayer(key, players[key]);
                }
                else
                {
                    string key = currPlayers[rand.Next(currPlayers.Count)];

                    RemovePlayer(key);
                }

                System.Threading.Thread.Sleep(200);
            }
        }

        static void AddPlayer(string name, string id)
        {
            currPlayers.Add(name);
            File.AppendAllText("test.log", "[0288.35] Rx: PLAYER: \"" + name + "\"<261><Nod> entered from 127.0.0.1 steamid " + id + "\n");
            Console.WriteLine("Player added");
        }

        static void RemovePlayer(string name)
        {
            currPlayers.Remove(name);
            File.AppendAllText("test.log", "[0300.05] Rx: PLAYER: \"" + name + "\"<261><Nod> disconnected\n");
            Console.WriteLine("Player removed");
        }
    }
}
