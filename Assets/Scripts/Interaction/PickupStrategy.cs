using System;
using UnityEngine;

namespace Interaction
{
    [Serializable]
    public class PickupStrategy : InteractionStrategy
    {
        public PickupStrategy(InteractableObject interactableObject) : base(interactableObject)
        {
        
        }
    
        public override void PerformInteraction()
        {
            //TODO Add item to inventory.
            Debug.Log("Pickup on " + InteractableObject.name);
        }
    }
}