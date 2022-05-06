using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Level {
  [Serializable]
  public class GroupEmitter : Marker, INotification {
    [SerializeField]
    public GameObject groupPrefab;

    [SerializeField]
    public Vector2 position;

    public PropertyName id => new("Group Spawn");
  }
}