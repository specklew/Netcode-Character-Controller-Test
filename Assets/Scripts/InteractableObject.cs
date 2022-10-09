using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Renderer[] _renderers;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        #if UNITY_EDITOR
                if(_renderers == null) Debug.LogError("GameObject doesn't contain any renderers in children!");
        #endif
    }

    public void EnableHighlight()
    {
        foreach (Renderer r in _renderers)
        {
            Outline.OutlineRenderers.Add(r);
        }
    }

    public void DisableHighlight()
    {
        foreach (Renderer r in _renderers)    
        {
            Outline.OutlineRenderers.Remove(r);
        }
    }
    
    public void Interact()
    {
        Debug.Log("Interaction!");
        //TODO: Interaction mechanism.
    }
}
