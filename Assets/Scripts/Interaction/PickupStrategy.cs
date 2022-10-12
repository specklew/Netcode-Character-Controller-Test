using System;
using UnityEngine;

namespace Interaction
{
    [Serializable]
    public class PickupStrategy : InteractionStrategy
    {
        public override void PerformInteraction()
        {
            //TODO Add item to inventory.
            Debug.Log("Pickup on " + gameObject);
            Destroy(gameObject);
        }
    }
}