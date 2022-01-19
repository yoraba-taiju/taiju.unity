using Donut.Unity;
using UnityEngine;

namespace Enemy.Bullet {
  public sealed class FixedSpeedAim : DonutBehaviour {
    [SerializeField] public float speed = 0.1f;
    [SerializeField] public float angle;
    private GameObject sora_;
    private Vector3 direction_;
    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      var trans = transform;
      var pos = trans.position;
      var vec = sora_.transform.position - pos;
      vec.z = 0.0f;
      vec /= vec.magnitude;
      direction_ = vec * speed;
      direction_ = Quaternion.AngleAxis(angle, Vector3.forward) * direction_;
    }

    protected override void OnUpdate() {
      transform.position += direction_;
    }
  }
}