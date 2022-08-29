using System;

namespace Networking
{
    [Serializable]
    public class ConnectionPayload
    {
        public string clientGuid;
        public int clientScene = -1;
        public string playerName;
    }
}
