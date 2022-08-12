using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Level {
  [Serializable]
  public class RushEmitter : Marker, INotification {
    [SerializeField] public GameObject rushPrefab;

    [SerializeField] public Vector2 position;

    public PropertyName id => new("Rush Spawn");
  }
}