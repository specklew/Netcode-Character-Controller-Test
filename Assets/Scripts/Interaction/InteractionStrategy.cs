using UnityEngine;

namespace Interaction
{
    [System.Serializable]
    public abstract class InteractionStrategy : MonoBehaviour
    {
        public abstract void PerformInteraction();
    }
}