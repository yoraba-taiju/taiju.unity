using UnityEngine;
using UnityEngine.Timeline;

namespace Level {
  public class SpawnEmitter : SignalEmitter
  {
    [SerializeField]
    public GameObject prefab;
  }
}