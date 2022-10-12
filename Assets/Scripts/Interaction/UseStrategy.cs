using System;
using UnityEngine;

namespace Interaction
{
    [Serializable]
    public class UseStrategy : InteractionStrategy
    {
        public override void PerformInteraction()
        {
            //TODO Send message to a script that will handle the mechanics.
            Debug.Log("Used " + gameObject + "!");
        }
    }
}