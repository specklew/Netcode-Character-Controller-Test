using Interaction;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractableObject))]
public class InteractableObjectEditor : Editor
{
    private bool _show;
    private InteractableObject _interactableObject;
    private InteractableObject.Strategies _lastStrategy;

    void OnEnable()
    {
        _interactableObject = (InteractableObject)target;
    }

    public override void OnInspectorGUI()
    {
        return;
        _interactableObject.strategy = (InteractableObject.Strategies)EditorGUILayout.EnumPopup("Strategy", _interactableObject.strategy);
        if(_interactableObject.strategy == _lastStrategy) return;
        _lastStrategy = _interactableObject.strategy;
        
        InteractionStrategy attachedStrategy = _interactableObject.GetComponent(typeof(InteractionStrategy)) as InteractionStrategy;
        if(attachedStrategy != null) DestroyImmediate(attachedStrategy);
           
        switch (_interactableObject.strategy)
        {
            case InteractableObject.Strategies.Pickup:
                Debug.Log("Wstawiam pickup i chuj");
                _interactableObject.AddComponent<PickupStrategy>();
                break;
            case InteractableObject.Strategies.Use:
                Debug.Log("Wstawiam use i chuj");
                _interactableObject.AddComponent<UseStrategy>();
                break;
        }
    }
}
