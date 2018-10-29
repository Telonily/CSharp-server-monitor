using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ServerMonitor
{
    class Monitor
    {

        private static readonly byte[] A2SInfo = {0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 0x6E, 0x67, 0x69,
            0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00};
        private static readonly byte[] A2SChallenge = {0xFF, 0xFF, 0xFF, 0xFF, 0x55, 0xFF, 0xFF, 0xFF, 0xFF};

        private IPAddress _ip;
        private string _sIp;
        private int _port;



        private readonly UdpClient _socket;

        private readonly IMonitorHandler _handler;
        private readonly PlayersHandler _playersHandler;
        
        public Monitor(IMonitorHandler handler, String ipPort)
        {
            _handler = handler;
            _playersHandler = new PlayersHandler(_handler);
            ParseIp(ipPort);
            _socket = new UdpClient(_sIp, _port);
            new Thread(Receiver).Start();


            SendPlayersRequest();
        }


        private void ParseIp(string ipPort)
        {
            try
            {
                int index = ipPort.IndexOf(":", StringComparison.Ordinal);
                _sIp = ipPort.Substring(0, index);
                string port = ipPort.Substring(index+1);
                _ip = IPAddress.Parse(_sIp);
                _port = Int32.Parse(port);
            }
            catch (Exception e)
            {
                throw new Exception("Bad IP Adress: "+e.Message);
            }
        }

        private void ParseData(byte[] buffer)
        {
            ReplyStream stream = new ReplyStream(new MemoryStream(buffer));
            if (stream.ReadInt() == -1)
            {
                HandleMessage(stream.ReadByte(), stream);
            }
        }

        public void SendInfoRequest()
        {
            _socket.Send(A2SInfo, A2SInfo.Length);
        }

        public void SendPlayersRequest()
        {
            _socket.Send(A2SChallenge, A2SChallenge.Length);
        }

        private void Receiver()
        {
            IPEndPoint endPoint = new IPEndPoint(_ip, 0);
            while (true)
            {
                try
                {

                    byte[] received = _socket.Receive(ref endPoint);
                    ParseData(received);
                }
                catch (Exception e)
                {
                    _handler.HandleError(e.Message);
                }
            }
        }

        private void HandleMessage(int code, ReplyStream stream)
        {
            switch (code)
            {
                case 'A':
                    HandleChallenge(stream);
                    break;
                case 'D':
                    _playersHandler.Process(stream);
                    break;
                case 'E':
                    HandleRules(stream);
                    break;
                case 'I':
                    HandleInformation(stream);
                    break;
                default:
                    HandleError(new Exception("Unexpected message: " + code));
                    break;
            }
        }

        private void HandleInformation(ReplyStream stream)
        {
            stream.ReadByte();
            string name = stream.ReadString();
            string map = stream.ReadString();
            string gameDirectory = stream.ReadString();
            string gameDescription = stream.ReadString();
            short applicationId = stream.ReadShort();
            int playersCount = stream.ReadUnsignedByte();
            int maximumPlayers = stream.ReadUnsignedByte();
            int botCount = stream.ReadByte();
            String type = "";
            switch ((int)stream.ReadByte())
            {
                case 'd': type = "Dedicated"; break;
                case 'l': type = "Listen"; break;
                case 'p': type = "TV"; break;
            }

            String os = "";
            switch ((int)stream.ReadByte())
            {
                case 'l': os = "Linux"; break;
                case 'w': os = "Windows"; break;
            }
            bool passwordRequired = stream.ReadByte() == 1;
            bool vacSecure = stream.ReadByte() == 1;
            String version = stream.ReadString();

            String overall = name + " " + playersCount + "/" + maximumPlayers + " " + map;

            NameValueCollection list = new NameValueCollection(5)
            {
                ["name"] = name,
                ["map"] = map,
                ["playersCount"] = playersCount.ToString(),
                ["maximumPlayers"] = maximumPlayers.ToString(),
                ["type"] = type,
                ["os"] = os
            };

            _handler.HandleServerInfo(list);
        }





        private void HandleChallenge(ReplyStream stream)
        {
            byte[] buff = new byte[9];
            for (int i = 0; i < 4; i++)
            {
                buff[i] = 0xFF;
            }
            buff[4] = 0x55;
            for (int i = 5; i < 9; i++)
            {
                buff[i] = stream.ReadByte();
            }
            _socket.Send(buff, buff.Length);
        }

        private void HandlePlayers(ReplyStream stream)
        {

            int playerCount = stream.ReadUnsignedByte();
            List<Player> players = new List<Player>(playerCount);
            for (int x = 0; x < playerCount; x++)
            {
                Player player = new Player(stream.ReadUnsignedByte());
                String name = stream.ReadString();
                if (name.Length > 0)
                {
                    player.Name = name;
                }
                player.Kills = stream.ReadInt();
                player.SecondsConnected = stream.ReadFloat();
                players.Add(player);
            }

            _handler.HandlePlayers(players);

        }

        private void HandleRules(ReplyStream stream)
        {

        }

        private void HandleError(Exception ex)
        {

        }


    }
}
