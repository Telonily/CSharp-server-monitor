using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor
{
    class PlayersHandler
    {
        private readonly IMonitorHandler _endpointHandler;


        public PlayersHandler(IMonitorHandler endpointHandler)
        {
            _endpointHandler = endpointHandler;
        }

        public void Process(ReplyStream stream)
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

            _endpointHandler.HandlePlayers(players);
        }

    }
}
