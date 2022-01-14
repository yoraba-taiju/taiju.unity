using System;
using UnityEngine;

namespace LevelEdit {
  public class Spawn : MonoBehaviour {
    [SerializeField]
    public GameObject target;
    private void OnBecameInvisible() {
    }

    private void OnBecameVisible() {
      Debug.Log("Appear");
      target.SetActive(true);
    }
  }
}