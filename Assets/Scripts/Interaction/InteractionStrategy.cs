using UnityEngine;

namespace Interaction
{
    [System.Serializable]
    public abstract class InteractionStrategy : MonoBehaviour
    {
        public bool IsSingleUse { get; protected set; }
        public abstract void PerformInteraction(ulong playerId);
    }
}