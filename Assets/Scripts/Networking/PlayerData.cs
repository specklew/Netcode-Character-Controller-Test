namespace Networking
{
    public struct PlayerData
    {
        public string PlayerName { get; private set; }
        public ulong ClientId { get; private set; }
        public bool IsAlive { get; set; }
        public PlayerData(string playerName, ulong clientId)
        {
            PlayerName = playerName;
            ClientId = clientId;
            IsAlive = true;
        }

        public PlayerData(string playerName, ulong clientId, bool isAlive)
        {
            PlayerName = playerName;
            ClientId = clientId;
            IsAlive = isAlive;
        }
    }
}