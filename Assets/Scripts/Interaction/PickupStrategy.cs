using System;
using UnityEngine;

namespace Interaction
{
    [Serializable]
    public class PickupStrategy : InteractionStrategy
    {
        public PickupStrategy()
        {
            IsSingleUse = true;
        }

        public override void PerformInteraction(ulong playerId)
        {
            //TODO Add item to inventory.
            Debug.Log("Pickup on " + gameObject);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}