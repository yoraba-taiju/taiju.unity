using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace Level {
  public class SpawnEmitter : SignalEmitter
  {
    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    public Vector2 position;

    public void OnEnable() {
      // Fill in empty signal asset.
      asset = ScriptableObject.CreateInstance<SignalAsset>();
    }
  }
}