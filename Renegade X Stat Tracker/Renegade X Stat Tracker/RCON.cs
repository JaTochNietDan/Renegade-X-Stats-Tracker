using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Renegade_X_Stat_Tracker
{
    class RCON
    {
        public static TcpClient tcp;
        private static NetworkStream stream;
        public static string gameVersion;
        private static Thread receiver;
        public static bool runReceiver = true;
        public static bool closing = false;

        public static bool Connect(string ip, int port, string pass)
        {
            Console.WriteLine("Attempting to connect to RCON for announcements.");

            try
            {
                tcp = new TcpClient();
                tcp.Connect(ip, port);
                stream = tcp.GetStream();
                byte[] array = new byte[1024];
                int count = stream.Read(array, 0, array.Length);
                string @string = Encoding.ASCII.GetString(array, 0, count);
                if (@string.Substring(0, 4) != "v001")
                {
                    Failed(string.Concat(new string[]
				    {
					    "Server is running a newer protocol (",
					    @string.Substring(0, 4),
					    ").",
					    Environment.NewLine,
					    "You need to download a new version of RxCommand."
				    }));
                }
                else
                {
                    gameVersion = @string.Substring(4);
                    gameVersion.Trim();
                    WriteLine('a' + pass);
                    count = stream.Read(array, 0, array.Length);
                    @string = Encoding.ASCII.GetString(array, 0, count);
                    if (@string.Substring(0, 1) != "a")
                    {
                        Failed(@string.Substring(1));
                    }
                    else
                    {
                        receiver = new Thread(new ThreadStart(ReceiveLoop));
                        receiver.Start();
                        while (!receiver.IsAlive)
                        {

                        }

                        Console.WriteLine("Successfully connected to RCON.");

                        return true;
                    }
                }
            }
            catch (SocketException)
            {
                Failed("Could not connect to server.");
                tcp = null;
            }

            return false;
        }
        public static void Subscribe()
        {
            WriteLine("s");
        }
        public static void Unsubscribe()
        {
            WriteLine("u");
        }
        public static void say(string message)
        {
            Command("say " + message);
        }
        public static void Command(string cmd)
        {
            WriteLine('c' + cmd);
        }
        private static void WriteLine(string msg)
        {
            try
            {
                msg += '\n';
                byte[] bytes = Encoding.ASCII.GetBytes(msg);
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (IOException)
            {
                CloseDown(true);
            }
        }
        private static void Failed(string error)
        {
            tcp.Close();
            tcp = null;
            stream = null;
            Console.WriteLine(error);
        }
        private static void ReceiveLoop()
        {
            byte[] array = new byte[1024];
            char[] separator = new char[]
		    {
			    '\n'
		    };
            
            while (runReceiver)
            {
                try
                {
                    int count = stream.Read(array, 0, array.Length);
                    string @string = Encoding.ASCII.GetString(array, 0, count);
                    string[] array2 = @string.Split(separator);
                    int num = 0;
                    while (num < array2.Length && array2[num] != "")
                    {
                        string a;
                        if ((a = array2[num].Substring(0, 1)) != null && a == "l")
                        {
                            Console.WriteLine(array2[num].Substring(1));
                        }
                        num++;
                    }
                    Thread.Sleep(10);
                }
                catch (IOException)
                {
                    CloseDown(true);
                    break;
                }
            }
        }
        public static void CloseDown(bool lostConnection = false)
        {
            if (closing)
            {
                return;
            }

            closing = true;
            runReceiver = false;

            if (tcp != null && tcp.Connected)
            {
                Util.StopAnnouncements();
                tcp.Close();
            }
        }
    }
}
