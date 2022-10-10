using UnityEngine;

public abstract class InteractionStrategy : MonoBehaviour
{
    public bool singleUse;
    public abstract void PerformInteraction();
}