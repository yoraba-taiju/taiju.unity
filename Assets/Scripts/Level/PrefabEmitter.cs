using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Level {
  [Serializable]
  public class PrefabEmitter : Marker, INotification {
    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    public Vector2 position;

    public PropertyName id => new("Prefab Spawn");
  }
}