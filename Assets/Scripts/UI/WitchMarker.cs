using System;
using UnityEngine;

namespace UI {
  public class WitchMarker: MonoBehaviour {
    [SerializeField] private GameObject witch;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Camera camera3d;
    [SerializeField] private Camera camera2d;

    private void LateUpdate() {
      transform.position = camera2d.ViewportToWorldPoint(camera3d.WorldToViewportPoint(witch.transform.position + offset));
    }
  }
}
