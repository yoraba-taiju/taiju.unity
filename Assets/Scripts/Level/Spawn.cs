using System;
using UnityEngine;

namespace Level {
  public class Spawn : MonoBehaviour {
    [SerializeField]
    public GameObject target;
    private void OnBecameInvisible() {
    }

    private void OnBecameVisible() {
      Instantiate(target);
    }
  }
}