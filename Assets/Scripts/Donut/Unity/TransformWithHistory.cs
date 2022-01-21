using System;
using System.Linq;
using System.Security.Cryptography;
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
    private Renderer[] renderers_;
    private bool wasVisible_ = false;
    private void Start() {
      var obj = GameObject.FindGameObjectWithTag("Clock");
      clockComponent_ = obj.GetComponent<ClockComponent>();
      clock_ = clockComponent_.Clock;
      var trans = transform;
      position_ = new Dense<Vector3>(clock_, trans.localPosition);
      scale_ = new Dense<Vector3>(clock_, trans.localScale);
      rot_ = new Dense<Quaternion>(clock_, trans.localRotation);
      renderers_ = GetComponentsInChildren<Renderer>();
      wasVisible_ = renderers_.All(it => it.isVisible);
    }

    private void Update() {
      if (wasVisible_) {
        if (!renderers_.Any(it => it.isVisible)) {
          Destroy(gameObject);
        }
      } else {
        wasVisible_ = renderers_.All(it => it.isVisible);
      }
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