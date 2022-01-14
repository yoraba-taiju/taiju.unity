using System;
using UnityEngine;

namespace LevelEdit {
  public class Appear : MonoBehaviour {
    private void OnBecameInvisible() {
      Debug.Log("Appear");
    }

    private void OnBecameVisible() {
      
    }
  }
}