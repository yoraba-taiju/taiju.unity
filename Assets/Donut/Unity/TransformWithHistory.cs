using System;
using Donut.Values;
using Unity.VisualScripting;
using UnityEngine;

namespace Donut.Unity {
  public class TransformWithHistory: MonoBehaviour {
    private Dense<Vector3> position_;
    private Dense<Vector3> scale_;
    private Dense<Quaternion> rot_;
    private ClockComponent clockComponent_;
    private Clock clock_;
    private void Start() {
      var obj = GameObject.FindGameObjectWithTag("Clock");
      clockComponent_ = obj.GetComponent<ClockComponent>();
      clock_ = clockComponent_.Clock;
      position_ = new Dense<Vector3>(clock_);
      scale_ = new Dense<Vector3>(clock_);
      rot_ = new Dense<Quaternion>(clock_);
    }

    private void Update() {
      var trans = transform;
      if (clock_.IsTicking) {
        position_.Value = trans.localPosition;
        scale_.Value = trans.localScale;
        rot_.Value = trans.localRotation;
      } else {
        trans.localPosition = position_.Value;
        trans.localScale = scale_.Value;
        trans.localRotation = rot_.Value;
      }
    }
  }
}