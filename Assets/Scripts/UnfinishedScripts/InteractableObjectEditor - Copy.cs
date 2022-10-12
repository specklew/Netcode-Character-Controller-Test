using Interaction;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//[CustomEditor(typeof(InteractableObject))]
public class InteractableObjectEditor : Editor
{
    // private bool _show;
    // private InteractableObject _interactableObject;
    // private InteractableObject.Strategies _lastStrategy;
    //
    // void OnEnable()
    // {
    //     _interactableObject = (InteractableObject)target;
    // }

    // public override VisualElement CreateInspectorGUI()
    // {
    //     // VisualElement root = new();
    //     //
    //     // var strategyField = new EnumField("Strategy") { bindingPath = "strategy" };
    //     // root.Add(strategyField);
    //     //
    //     // SerializedProperty strategyProperty = serializedObject.FindProperty("strategy");
    //     //
    //     // UpdateTheComponents(strategyProperty);
    //     
    //     //root.TrackPropertyValue(strategyProperty, UpdateTheComponents);
    //     
    //     // return root;
    // }

    private void UpdateTheComponents(SerializedProperty serializedProperty)
    {
        //Debug.Log("boo!");
        // var strategy = (InteractableObject.Strategies)serializedProperty.enumValueIndex;
        //
        // var attachedStrategy = _interactableObject.GetComponent(typeof(InteractionStrategy)) as InteractionStrategy;
        // if(attachedStrategy != null) DestroyImmediate(attachedStrategy);
        //
        // switch (strategy)
        // {
        //     case InteractableObject.Strategies.Pickup:
        //         _interactableObject.AddComponent <PickupStrategy>();
        //         break;
        //     case InteractableObject.Strategies.Use:
        //         _interactableObject.AddComponent <UseStrategy>();
        //         break;
        // }
        //Debug.Log("Zmiana!");
    }

    public override void OnInspectorGUI()
    {
        // return;
        // _interactableObject.strategy = (InteractableObject.Strategies)EditorGUILayout.EnumPopup("Strategy", _interactableObject.strategy);
        // if(_interactableObject.strategy == _lastStrategy) return;
        // _lastStrategy = _interactableObject.strategy;
        //
        // InteractionStrategy attachedStrategy = _interactableObject.GetComponent(typeof(InteractionStrategy)) as InteractionStrategy;
        // if(attachedStrategy != null) DestroyImmediate(attachedStrategy);
        //    
        // switch (_interactableObject.strategy)
        // {
        //     case InteractableObject.Strategies.Pickup:
        //         Debug.Log("Wstawiam pickup i chuj");
        //         _interactableObject.AddComponent<PickupStrategy>();
        //         break;
        //     case InteractableObject.Strategies.Use:
        //         Debug.Log("Wstawiam use i chuj");
        //         _interactableObject.AddComponent<UseStrategy>();
        //         break;
        // }
    }
}
