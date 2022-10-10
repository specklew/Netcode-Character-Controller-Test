using UnityEngine;

public class UseStrategy : InteractionStrategy
{
    public override void PerformInteraction()
    {
        Debug.Log("Used " + gameObject + "!");
    }
}