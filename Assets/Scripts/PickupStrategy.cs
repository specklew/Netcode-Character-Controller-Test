using UnityEngine;

public class PickupStrategy : InteractionStrategy
{
    private PickupStrategy()
    {
        singleUse = true;
    }
    public override void PerformInteraction()
    {
        Debug.Log("Pickup!");
        Destroy(gameObject);
    }
}