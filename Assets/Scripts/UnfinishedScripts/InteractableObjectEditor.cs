using UnityEditor;
using UnityEditor.Rendering;
using UnityEditorInternal;
using UnityEngine;

namespace Interaction
{
    //TODO: Change the editor window to be similar to CustomPassVolume window. (CustomPassVolumeEditor.cs)
    //[CustomEditor(typeof(InteractableObject))]
    // public class InteractableObjectEditor : Editor
    // {
    //     private InteractableObject _interactableObject;
    //     private ReorderableList _strategiesList;
    //
    //     private const string listName = "Interaction strategies";
    //
    //     private class SerializableInteractableObject
    //     {
    //         public SerializedProperty strategies;
    //     }
    //
    //     private SerializableInteractableObject _serializableInteractableObject;
    //     
    //     private void OnEnable()
    //     {
    //         _interactableObject = target as InteractableObject;
    //
    //         using PropertyFetcher<InteractableObject> o = new(serializedObject);
    //         _serializableInteractableObject = new SerializableInteractableObject
    //         {
    //             strategies = o.Find(x => x.strategies)
    //         };
    //     }
    //
    //     public override void OnInspectorGUI()
    //     {
    //         DrawCommandReorderableList();
    //     }
    //
    //     private void DrawCommandReorderableList()
    //     {
    //         for (int i = 0; i < _serializableInteractableObject.strategies.arraySize; i++)
    //         {
    //             if (_serializableInteractableObject.strategies.GetArrayElementAtIndex(i) != null) continue;
    //             _serializableInteractableObject.strategies.DeleteArrayElementAtIndex(i);
    //             serializedObject.ApplyModifiedProperties();
    //             i++;
    //         }
    //
    //         float strategiesListHeight = _strategiesList.GetHeight();
    //         Rect strategiesRect = EditorGUILayout.GetControlRect(false, strategiesListHeight);
    //         EditorGUI.BeginProperty(strategiesRect, GUIContent.none, _serializableInteractableObject.strategies);
    //         {
    //             EditorGUILayout.BeginHorizontal();
    //             _strategiesList.DoList(strategiesRect);
    //             EditorGUILayout.EndVertical();
    //         }
    //         EditorGUI.EndProperty();
    //     }
    //
    //     private void CreateReorderableList(SerializedProperty strategyList)
    //     {
    //         _strategiesList = new ReorderableList(strategyList.serializedObject, strategyList);
    //
    //         _strategiesList.drawHeaderCallback = (rect) =>
    //         {
    //             EditorGUI.LabelField(rect, listName, EditorStyles.largeLabel);
    //         };
    //
    //         _strategiesList.multiSelect = false;
    //         _strategiesList.drawElementCallback = (rect, index, active, focused) =>
    //         {
    //             EditorGUI.BeginChangeCheck();
    //
    //             strategyList.serializedObject.ApplyModifiedProperties();
    //             SerializedProperty strategy = strategyList.GetArrayElementAtIndex(index);
    //             strategy.managedReferenceValue = _interactableObject.strategies[index];
    //
    //         };
    //     }
    // }
}