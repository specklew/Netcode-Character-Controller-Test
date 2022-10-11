using System;
using UnityEngine;

namespace Interaction
{
    [Serializable]
    public class UseStrategy : InteractionStrategy
    {
        public UseStrategy(InteractableObject interactableObject) : base(interactableObject)
        {
        }
    
        public override void PerformInteraction()
        {
            Debug.Log("Used " + InteractableObject.name + "!");
        }
    }
}