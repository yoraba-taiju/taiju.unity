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
      var vec = sora_.transform.position - trans.position;
      vec /= vec.magnitude;
      trans.position += vec * 0.11f;
      Debug.Log(vec);
    }
  }
}