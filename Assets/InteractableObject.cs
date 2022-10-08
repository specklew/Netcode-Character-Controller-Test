using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Renderer[] _renderers;
    
    private Material _outlineMaterial;
    private Material _emptyMaterial;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();

        _outlineMaterial = Resources.Load<Material>("Materials/OutlineMaterial");
        _emptyMaterial = Resources.Load<Material>("Materials/EmptyMaterial");
    }

    private void Start()
    {
        AddOutlineMaterialToRenderer();
    }

    private void AddOutlineMaterialToRenderer()
    {
        foreach (Renderer r in _renderers)
        {
            Material[] materials = new Material[r.materials.Length + 1];
        
            for (int i = 1; i <= r.materials.Length; i++)
            {
                materials[i] = r.materials[i - 1];
            }
        
            r.materials = materials;
            r.material = _emptyMaterial;
        }
    }

    public void EnableHighlight()
    {
        foreach (Renderer r in _renderers)
        {
            r.material = _outlineMaterial;  
        }
    }

    public void DisableHighlight()
    {
        foreach (Renderer r in _renderers)    
        {
            r.material = _emptyMaterial;
        }
    }
    
    public void Interact()
    {
        //TODO: Interaction mechanism.
    }
}
