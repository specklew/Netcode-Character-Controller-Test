using UnityEngine;

namespace Interaction
{
    [System.Serializable]
    public abstract class InteractionStrategy : MonoBehaviour
    {
        protected readonly InteractableObject InteractableObject;
        protected InteractionStrategy(InteractableObject interactableObject)
        {
            InteractableObject = interactableObject;
        }

        public abstract void PerformInteraction();
    }
}