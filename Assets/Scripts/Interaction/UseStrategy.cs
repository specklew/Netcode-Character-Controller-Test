using System;
using UnityEngine;

namespace Interaction
{
    [Serializable]
    public class UseStrategy : InteractionStrategy
    {
        public UseStrategy()
        {
            IsSingleUse = false;
        }

        public override void PerformInteraction(ulong playerId)
        {
            //TODO Send message to a script that will handle the mechanics.
            Debug.Log("Used " + gameObject + "!");
        }
    }
}