using JetBrains.Annotations;

namespace PlayerScripts
{
    public interface IClickable
    {
        [UsedImplicitly]
        void ClickedByPlayerServerRpc(ulong playerId);
    }
}