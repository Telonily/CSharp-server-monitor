using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor
{
    public class Player
    {
        public string Name;
        public int Kills;
        public float SecondsConnected;
        private int _index;

        public Player(int index)
        {
            _index = index;
        }

        public Player(string name, int kills, float secondsConnected, int index)
        {
            Name = name;
            Kills = kills;
            SecondsConnected = secondsConnected;
            _index = index;
        }



        public new String ToString()
        {
            StringBuilder sb = new StringBuilder(80);
            sb.Append(Name);
            for (int i = 0; i < 35 - Name.Length; i++)
                sb.Append(" ");
            sb.Append(Kills);
            for (int i = 0; i < 5 - Kills.ToString().Length; i++)
                sb.Append(" ");

            int sec = (int)SecondsConnected % 60;
            int min = (int)SecondsConnected / 60;

            sb.Append((min / 10) < 1 ? "0" + min : min+"");
            sb.Append(":");
            sb.Append((sec / 10) < 1 ? "0" + sec : sec+"");
            return sb.ToString();
        }


        public string GetMinConnected()
        {
            int min = (int)SecondsConnected / 60;
            return (min / 10) < 1 ? "0" + min : min+"";
        }

        public string GetSecConnected()
        {
            int sec = (int)SecondsConnected % 60;
            return (sec / 10) < 1 ? "0" + sec : sec+"";
        }

    }
}
