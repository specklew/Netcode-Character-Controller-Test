using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Renderer _renderer;
    
    private Material _outlineMaterial;
    private Material _emptyMaterial;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();

        _outlineMaterial = Resources.Load<Material>("Materials/OutlineMaterial");
        _emptyMaterial = Resources.Load<Material>("Materials/EmptyMaterial");
    }

    private void Start()
    {
        AddOutlineMaterialToRenderer();
    }

    private void AddOutlineMaterialToRenderer()
    {
        Material[] materials = new Material[_renderer.materials.Length + 1];
        
        for (int i = 1; i <= _renderer.materials.Length; i++)
        {
            materials[i] = _renderer.materials[i - 1];
        }
        
        _renderer.materials = materials;
        _renderer.material = _emptyMaterial;
    }

    public void EnableHighlight()
    {
        _renderer.material = _outlineMaterial;
    }

    public void DisableHighlight()
    {
        _renderer.material = _emptyMaterial;
    }
    
    public void Interact()
    {
        //TODO: Interaction mechanism.
    }
}
