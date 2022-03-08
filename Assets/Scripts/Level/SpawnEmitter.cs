using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Level {
  [Serializable]
  public class SpawnEmitter : Marker, INotification {
    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    public Vector2 position;

    public PropertyName id => new("Enemy Spawn");
  }
}