using System;
using UnityEngine;

namespace UI {
  public class WitchMarker: MonoBehaviour {
    [SerializeField] private GameObject witch_;
    [SerializeField] private Vector3 offset_;
    [SerializeField] private Camera Camera3d_;
    [SerializeField] private Camera Camera2d_;

    private void Start() {
      
    }

    private void Update() {
      var pos = Camera3d_.WorldToViewportPoint(witch_.transform.position + offset_);
      pos.z = 10;
      transform.position = Camera2d_.ViewportToWorldPoint(pos);
    }
  }
}