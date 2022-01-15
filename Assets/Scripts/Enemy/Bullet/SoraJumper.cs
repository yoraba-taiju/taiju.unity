using Donut.Unity;
using UnityEngine;

namespace Enemy.Bullet {
  public sealed class SoraJumper : DonutBehaviour {
    private GameObject sora_;
    private Vector3 speed_;
    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      var trans = transform;
      var pos = trans.position;
      var vec = sora_.transform.position - pos;
      vec /= vec.magnitude;
      speed_ = vec;
    }

    protected override void OnUpdate() {
      transform.position += speed_ * 0.5f;
    }
  }
}