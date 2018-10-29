using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor
{
    interface IMonitorHandler
    {
        void HandleServerInfo(NameValueCollection message);
        void HandlePlayers(List<Player> players);
        void HandleError(String mes);

    }
}
