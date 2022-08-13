using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts {
  [CustomEditor(typeof(MeshRenderer))]
  public class MeshRendererSortingEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();
      var renderer = (target as MeshRenderer)!;
      var layers = SortingLayer.layers;

      EditorGUILayout.BeginHorizontal();
      EditorGUI.BeginChangeCheck();
      var newId = DrawSortingLayersPopup(renderer!.sortingLayerID);
      if (EditorGUI.EndChangeCheck()) {
        renderer.sortingLayerID = newId;
      }

      EditorGUILayout.EndHorizontal();

      EditorGUILayout.BeginHorizontal();
      EditorGUI.BeginChangeCheck();
      var order = EditorGUILayout.IntField("Sorting Order", renderer.sortingOrder);
      if (EditorGUI.EndChangeCheck()) {
        renderer.sortingOrder = order;
      }

      EditorGUILayout.EndHorizontal();
    }

    int DrawSortingLayersPopup(int layerID) {
      var layers = SortingLayer.layers;
      var names = layers.Select(l => l.name).ToArray();
      if (!SortingLayer.IsValid(layerID)) {
        layerID = layers[0].id;
      }

      var layerValue = SortingLayer.GetLayerValueFromID(layerID);
      var newLayerValue = EditorGUILayout.Popup("Sorting Layer", layerValue, names);
      return layers[newLayerValue].id;
    }
  }
}