using Donut.Unity;
using UnityEngine;

namespace Enemy.Bullet {
  public sealed class SoraTracker : DonutBehaviour {
    private GameObject sora_;
    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
    }

    protected override void OnUpdate() {
      var trans = transform;
      var pos = trans.position;
      var vec = sora_.transform.position - pos;
      vec /= vec.magnitude;
      trans.position = pos + vec * 0.11f;
    }
  }
}